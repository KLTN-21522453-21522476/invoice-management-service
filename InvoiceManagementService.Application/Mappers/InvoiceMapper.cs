using InvoiceManagementService.Application.Contracts.DTO;
using InvoiceManagementService.Domain.Entities;
using InvoiceManagementService.Shared.Enums;


namespace InvoiceManagementService.Application.Mappers;

public class InvoiceMapper
{
    public Invoice MapDtoToInvoice(InvoiceDto dto)
    {
        var now = DateTime.UtcNow;
        return new Invoice
        {
            Id = dto.Id ?? Guid.Empty,
            GroupId = dto.GroupId ?? Guid.Empty,
            ApprovedBy = dto.ApprovedBy ?? Guid.Empty,
            SubmittedBy = dto.SubmittedBy ?? Guid.Empty,
            InvoiceNumber = dto.InvoiceNumber ?? string.Empty,
            InvoiceDate = dto.CreatedDate ?? now,
            Address = dto.Address ?? string.Empty,
            Amount = dto.TotalAmount ?? 0,
            ImagePath = dto.ImageUrl ?? string.Empty,
            StoreName = dto.StoreName ?? string.Empty,
            Status = (Status)dto.Status,
            CreatedAt = dto.CreatedDate ?? now,
            UpdatedAt = dto.UpdatedAt ?? now,
            Items = dto.Items?.Select(x => new Item
            {
                Id = x.Id ?? Guid.NewGuid(),
                Name = x.Item ?? string.Empty,
                Price = x.Price ?? 0,
                Quantity = x.Quantity ?? 0,
                Total = (x.Price ?? 0) * (x.Quantity ?? 0),
                CreatedAt = now,
                UpdatedAt = now
            }).ToList() ?? new List<Item>()
        };
    }

    public InvoiceDto MapInvoiceToDto(Invoice entity)
    {
        return new InvoiceDto
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            ApprovedBy = entity.ApprovedBy,
            SubmittedBy = entity.SubmittedBy,
            InvoiceNumber = entity.InvoiceNumber,
            CreatedDate = entity.InvoiceDate,
            Address = entity.Address,
            TotalAmount = entity.Amount,
            ImageUrl = entity.ImagePath,
            StoreName = entity.StoreName,
            Status = (Status)entity.Status,
            UpdatedAt = entity.UpdatedAt,
            Items = entity.Items?.Select(x => new ItemDto
            {
                Id = x.Id,
                Item = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                Invoice = x.InvoiceId,
            }).ToList()
        };
    }
}
