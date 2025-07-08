using Microsoft.EntityFrameworkCore;
using InvoiceManagementService.Domain.Models;

namespace InvoiceManagementService.Infrastructure.Data.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Item> Items { get; set; }
}