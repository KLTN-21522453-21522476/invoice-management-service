using InvoiceManagementService.Application.Contracts.DTO;
using InvoiceManagementService.Application.Contracts.Dtos;

namespace InvoiceManagementService.Application.Contracts.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto);
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);
    Task<List<InvoiceDto>> GetInvoicesByGroupAsync(InvoiceGroupQueryParametersDto invoiceGroupQueryParametersDto);
}
