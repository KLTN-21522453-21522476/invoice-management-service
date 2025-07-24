using InvoiceManagementService.Domain.Entities;
using InvoiceManagementService.Domain.Interfaces;
using InvoiceManagementService.Infrastructure.Data.Context;
using InvoiceManagementService.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagementService.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<(List<Invoice> invoices, int totalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = _context.Invoices.Include(i => i.Items);

        var totalCount = await query.CountAsync();
        var invoices = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (invoices, totalCount);
    }

    public async Task<Invoice> AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice?> UpdateAsync(Invoice invoice)
    {
        var existingInvoice = await GetByIdAsync(invoice.Id);
        if (existingInvoice == null) return null;

        _context.Entry(existingInvoice).CurrentValues.SetValues(invoice);

        if (invoice.Items != null)
        {
            _context.Items.RemoveRange(existingInvoice.Items);

            foreach (var item in invoice.Items)
            {
                item.InvoiceId = invoice.Id;
                _context.Items.Add(item);
            }
        }

        await _context.SaveChangesAsync();
        return existingInvoice;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var invoice = await GetByIdAsync(id);
        if (invoice == null) return false;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Invoice>> GetByStatusAsync(Status status)
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    // Method bị thiếu - đã được thêm vào
    public async Task<List<Invoice>> GetByGroupIdAsync(Guid groupId)
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .Where(i => i.GroupId == groupId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Invoices.AnyAsync(i => i.Id == id);
    }

    public async Task<bool> InvoiceNumberExistsAsync(string invoiceNumber)
    {
        return await _context.Invoices.AnyAsync(i => i.InvoiceNumber == invoiceNumber);
    }
}
