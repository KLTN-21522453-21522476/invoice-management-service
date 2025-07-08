using InvoiceManagementService.Shared.Enums;

namespace InvoiceManagementService.Domain.Models;
public class Invoice
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public Guid ApprovedBy { get; set; }
    public Guid RejectedBy { get; set; }
    public Guid Model { get; set; }
    public Guid SubmittedBy { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string Address { get; set; }
    public decimal Amount { get; set; }
    public string ImagePath { get; set; }
    public string StoreName { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Item> Items { get; set; }
}
