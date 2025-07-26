using Microsoft.AspNetCore.Http;

namespace InvoiceManagementService.Application.Contracts.DTO;

public class CreateInvoiceRequest
{
    public InvoiceDto Invoice { get; set; } = new();
    public IFormFile? File { get; set; }
}
