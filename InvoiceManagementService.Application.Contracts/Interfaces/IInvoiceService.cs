using InvoiceManagementService.Application.Contracts.DTO;
using Microsoft.AspNetCore.Http;

namespace InvoiceManagementService.Application.Contracts.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto, IFormFile invoiceImage);
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);
    Task<List<InvoiceDto>> GetInvoicesByGroupAsync(InvoiceGroupQueryParametersDto invoiceGroupQueryParametersDto);
    Task<InvoiceDto> UpdateInvoiceAsync(InvoiceDto invoiceDto);
    Task<bool> DeleteInvoiceAsync(Guid id);
}
