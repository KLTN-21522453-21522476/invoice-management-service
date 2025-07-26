using InvoiceManagementService.Application.Contracts.DTO;

namespace InvoiceManagementService.Application.Contracts.Interfaces;

public interface IAppwriteStorageService
{
    Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileId, CancellationToken cancellationToken = default);
    Task<string> GetFileUrlAsync(string fileId, CancellationToken cancellationToken = default);
    Task<bool> CheckConnectionAsync(CancellationToken cancellationToken = default);
}
