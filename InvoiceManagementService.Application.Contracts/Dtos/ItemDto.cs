namespace InvoiceManagementService.Application.Contracts.DTOs
{
    public class ItemDto
    {
        public Guid? Id { get; set; }
        public string? Item { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public Guid? Invoice { get; set; }
    }
}
