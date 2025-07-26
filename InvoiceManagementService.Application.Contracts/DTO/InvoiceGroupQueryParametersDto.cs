using InvoiceManagementService.Shared.Enums;

namespace InvoiceManagementService.Application.Contracts.DTO;

public class InvoiceGroupQueryParametersDto
{
    public Guid GroupId { get; set; }
    public Guid? ApprovedBy { get; set; }
    public Guid? SubmittedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? StoreName { get; set; }
    public Status? Status { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}
