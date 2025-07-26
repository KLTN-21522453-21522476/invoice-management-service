using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
using InvoiceManagementService.Application.Contracts.DTO;
using InvoiceManagementService.Application.Contracts.Interfaces;
using InvoiceManagementService.Domain.Exceptions;
using InvoiceManagementService.Shared.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvoiceManagementService.Infrastructure.AppwriteService;

public class AppwriteStorageService : IAppwriteStorageService
{
    private readonly Storage _storage;
    private readonly ILogger<AppwriteStorageService> _logger;
    private readonly AppwriteConfig _settings;

    public AppwriteStorageService(
        Client appwriteClient,
        ILogger<AppwriteStorageService> logger,
        IOptions<AppwriteConfig> settings)
    {
        //_storage = new Storage(appwriteClient ?? throw new ArgumentNullException(nameof(appwriteClient)));
        _storage = new Storage(appwriteClient);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        // Input validation
        ValidateUploadInputs(fileStream, fileName);
        if (fileStream.CanSeek)
        {
            fileStream.Position = 0;
        }
        try
        {
            _logger.LogInformation("Starting file upload: {FileName}", fileName);

            var fileId = GenerateUniqueFileId();
            var mimeType = GetMimeTypeForFile(fileName);
            var inputFile = InputFile.FromStream(fileStream, fileName, mimeType);
            var file = await _storage.CreateFile(
                bucketId: _settings.BucketId,
                fileId: fileId,
                file: inputFile,
                permissions: new List<string>
                {
                    Permission.Read(Role.Any()),
                }
            );
            string fileUrl = await GetFileUrlAsync(file.Id, cancellationToken);

            FileUploadResult result = new FileUploadResult
            {
                FileId = file.Id,
                FileUrl = fileUrl,
            };

            _logger.LogInformation("File uploaded successfully with ID: {FileId}", result.FileId);
            return result;
        }
        catch (AppwriteException ex)
        {
            LogAppwriteError(ex, "upload", fileName);
            throw new StorageOperationException($"Failed to upload file: {fileName}", ex);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("File upload was cancelled: {FileName}", fileName);
            throw;
        }
    }

    public async Task<byte[]> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        ValidateFileId(fileId);

        try
        {
            _logger.LogInformation("Starting file download: {FileId}", fileId);

            var result = await _storage.GetFileDownload(
                bucketId: _settings.BucketId,
                fileId: fileId
            );

            _logger.LogInformation("File downloaded successfully: {FileId}, Size: {Size} bytes",
                fileId, result.Length);
            return result;
        }
        catch (AppwriteException ex)
        {
            LogAppwriteError(ex, "download", fileId);
            throw new StorageOperationException($"Failed to download file: {fileId}", ex);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("File download was cancelled: {FileId}", fileId);
            throw;
        }
    }

    public async Task DeleteFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        ValidateFileId(fileId);

        try
        {
            _logger.LogInformation("Starting file deletion: {FileId}", fileId);

            await _storage.DeleteFile(
                bucketId: _settings.BucketId,
                fileId: fileId
            );

            _logger.LogInformation("File deleted successfully: {FileId}", fileId);
        }
        catch (AppwriteException ex)
        {
            LogAppwriteError(ex, "delete", fileId);
            throw new StorageOperationException($"Failed to delete file: {fileId}", ex);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("File deletion was cancelled: {FileId}", fileId);
            throw;
        }
    }

    public Task<string> GetFileUrlAsync(string fileId, CancellationToken cancellationToken = default)
    {
        ValidateFileId(fileId);

        try
        {
            _logger.LogDebug("Getting file URL: {FileId}", fileId);
            var url = $"{_settings.Endpoint}/storage/buckets/{_settings.BucketId}/files/{fileId}/view?project={_settings.ProjectId}";
            _logger.LogDebug("File URL retrieved successfully: {FileId}", fileId);
            return Task.FromResult(url); ;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Get file URL was cancelled: {FileId}", fileId);
            throw;
        }
    }

    #region Private Helper Methods

    private static void ValidateUploadInputs(Stream fileStream, string fileName)
    {
        if (fileStream == null)
            throw new ArgumentNullException(nameof(fileStream));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

        if (!fileStream.CanRead)
            throw new ArgumentException("Stream must be readable", nameof(fileStream));
    }

    private static void ValidateFileId(string fileId)
    {
        if (string.IsNullOrWhiteSpace(fileId))
            throw new ArgumentException("File ID cannot be null or empty", nameof(fileId));
    }

    private static string GenerateUniqueFileId() => ID.Unique();

    private static string GetMimeTypeForFile(string fileName)
    {
        return MimeTypeHelper.GetMimeType(fileName) ?? "application/octet-stream";
    }

    private void LogAppwriteError(AppwriteException ex, string operation, string identifier)
    {
        _logger.LogError(ex,
            "Failed to {Operation} file: {Identifier}. Appwrite Error: {ErrorCode} - {ErrorMessage}",
            operation, identifier, ex.Code, ex.Message);
    }

    public async Task<bool> CheckConnectionAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking Appwrite storage connection...");
        try
        {
            await _storage.ListFiles(
                bucketId: _settings.BucketId,
                queries: new List<string> { Query.Limit(1) }
                );

            _logger.LogInformation("Appwrite storage connection check successful.");
            return true;
        }
        catch (AppwriteException ex)
        {
            _logger.LogError(ex, "Appwrite storage connection check failed. Appwrite Error: {ErrorCode} - {ErrorMessage}", ex.Code, ex.Message);
            return false;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An unexpected error occurred while checking Appwrite storage connection.");
            return false;
        }
    }

    #endregion
}
