using InvoiceManagementService.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceManagementService.Domain.Entities;
public class Invoice
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("group_id")]
    public Guid GroupId { get; set; }

    [Column("approved_by")]
    public Guid ApprovedBy { get; set; }

    [Column("rejected_by")]
    public Guid RejectedBy { get; set; }

    [Column("model")]
    public Guid Model { get; set; }

    [Column("submitted_by")]
    public Guid SubmittedBy { get; set; }

    [Column("invoice_number")]
    public string InvoiceNumber { get; set; }

    [Column("invoice_date")]
    public DateTime InvoiceDate { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("file_name")]
    public string FileName { get; set; }

    [Column("file_id")]
    public string FileId { get; set; }

    [Column("image_path")]
    public string ImagePath { get; set; }

    [Column("store_name")]
    public string StoreName { get; set; }

    [Column("status")]
    public Status Status { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public List<Item> Items { get; set; }
}