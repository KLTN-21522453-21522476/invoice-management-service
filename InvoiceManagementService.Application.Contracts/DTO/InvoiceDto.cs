using InvoiceManagementService.Shared.Enums;

namespace InvoiceManagementService.Application.Contracts.DTO;

public class InvoiceDto
{
    public Guid? Id { get; set; }
    public List<ItemDto>? Items { get; set; }
    public string? InvoiceNumber { get; set; }
    public Guid? GroupId { get; set; }
    public string? Model { get; set; }
    public string? Address { get; set; }
    public string? FileName { get; set; }
    public string? FileId { get; set; }
    public string? StoreName { get; set; }
    public Status Status { get; set; }
    public Guid? ApprovedBy { get; set; }
    public Guid? SubmittedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? ImageUrl { get; set; }
}
