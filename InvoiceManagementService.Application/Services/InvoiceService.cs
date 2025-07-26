using InvoiceManagementService.Application.Contracts.DTO;
using InvoiceManagementService.Application.Contracts.Interfaces;
using InvoiceManagementService.Application.Mappers;
using InvoiceManagementService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InvoiceManagementService.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAppwriteStorageService _appwriteStorageService;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        IAppwriteStorageService appwriteStorageService,
        ILogger<InvoiceService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _appwriteStorageService = appwriteStorageService;
        _logger = logger;
    }

    // Bổ sung: Get invoice by id
    public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting invoice with Id: {InvoiceId}", id);
            
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice with Id: {InvoiceId} not found", id);
                return null;
            }

            _logger.LogInformation("Invoice with Id: {InvoiceId} retrieved successfully", id);
            
            var mapper = new InvoiceMapper();
            return mapper.MapInvoiceToDto(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving invoice with Id: {InvoiceId}", id);
            throw;
        }
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto, IFormFile invoiceImage)
    {
        try
        {
            _logger.LogInformation("Creating invoice with InvoiceNumber: {InvoiceNumber}", invoiceDto.InvoiceNumber);
            InvoiceMapper mapper = new InvoiceMapper();
            if (invoiceImage != null && invoiceImage.Length > 0)
            {
                _logger.LogInformation("Uploading invoice image for InvoiceNumber: {InvoiceNumber}", invoiceDto.InvoiceNumber);

                using (var stream = invoiceImage.OpenReadStream())
                {
                    var result = await _appwriteStorageService.UploadFileAsync(stream, invoiceImage.FileName);
                    invoiceDto.ImageUrl = result.FileUrl;
                    invoiceDto.FileName = invoiceImage.FileName;
                    invoiceDto.FileId = result.FileId;
                }

                _logger.LogInformation("Invoice image uploaded successfully for InvoiceNumber: {InvoiceNumber}", invoiceDto.InvoiceNumber);
            }
            var invoice = mapper.MapDtoToInvoice(invoiceDto);
            await _invoiceRepository.AddAsync(invoice);
            _logger.LogInformation("Invoice {InvoiceId} created successfully", invoice.Id);
            return mapper.MapInvoiceToDto(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating invoice with InvoiceNumber: {InvoiceNumber}", invoiceDto.InvoiceNumber);
            throw;
        }
    }

    public async Task<List<InvoiceDto>> GetInvoicesByGroupAsync(InvoiceGroupQueryParametersDto invoiceGroupQueryParametersDto)
    {
        try
        {
            _logger.LogInformation("GetInvoicesByGroupAsync started at {DateTime} for GroupId: {GroupId}", 
                DateTime.Now, invoiceGroupQueryParametersDto.GroupId);
            
            var invoices = await _invoiceRepository.GetByGroupIdAsync(invoiceGroupQueryParametersDto.GroupId);

            _logger.LogInformation("Retrieved {Count} invoices for GroupId: {GroupId}", 
                invoices.Count, invoiceGroupQueryParametersDto.GroupId);
            
            var mapper = new InvoiceMapper();
            var invoiceDtos = invoices.Select(invoice => mapper.MapInvoiceToDto(invoice)).ToList();
            
            return invoiceDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving invoices for GroupId: {GroupId}", 
                invoiceGroupQueryParametersDto.GroupId);
            throw;
        }
    }

    public async Task<InvoiceDto> UpdateInvoiceAsync(InvoiceDto invoiceDto)
    {
        try
        {
            _logger.LogInformation("Updating invoice with Id: {InvoiceId}", invoiceDto.Id);
            InvoiceMapper mapper = new InvoiceMapper();
            var invoice = mapper.MapDtoToInvoice(invoiceDto);
            invoice = await _invoiceRepository.UpdateAsync(invoice);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice with Id: {InvoiceId} not found for update", invoiceDto.Id);
                return null;
            }
            _logger.LogInformation("Invoice {InvoiceId} updated successfully", invoice.Id);
            return mapper.MapInvoiceToDto(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating invoice with Id: {InvoiceId}", invoiceDto.Id);
            throw;
        }
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting invoice with Id: {InvoiceId}", id);
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice with Id: {InvoiceId} not found for deletion", id);
                return false;
            }
            var result = await _invoiceRepository.DeleteAsync(id);
            if (result)
            {
                await _appwriteStorageService.DeleteFileAsync(invoice.FileId);
                _logger.LogInformation("Invoice with Id: {InvoiceId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete invoice with Id: {InvoiceId}", id);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting invoice with Id: {InvoiceId}", id);
            throw;
        }
    }
}
