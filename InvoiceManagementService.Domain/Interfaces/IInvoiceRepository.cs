using InvoiceManagementService.Domain.Entities;

using InvoiceManagementService.Shared.Enums;

namespace InvoiceManagementService.Domain.Interfaces;
public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<List<Invoice>> GetAllAsync();
    Task<(List<Invoice> invoices, int totalCount)> GetPagedAsync(int pageNumber = 1, int pageSize = 10);
    Task<Invoice> AddAsync(Invoice invoice);
    Task<Invoice?> UpdateAsync(Invoice invoice);
    Task<bool> DeleteAsync(Guid id);
    Task<List<Invoice>> GetByStatusAsync(Status status);
    Task<List<Invoice>> GetByGroupIdAsync(Guid groupId);
    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> InvoiceNumberExistsAsync(string invoiceNumber);
}
