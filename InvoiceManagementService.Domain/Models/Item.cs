namespace InvoiceManagementService.Domain.Models;
public class Item
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
