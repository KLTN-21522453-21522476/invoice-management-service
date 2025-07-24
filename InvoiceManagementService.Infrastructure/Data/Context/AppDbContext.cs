using Microsoft.EntityFrameworkCore;
using InvoiceManagementService.Domain.Entities;

namespace InvoiceManagementService.Infrastructure.Data.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Or specific configurations
        // modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        // modelBuilder.ApplyConfiguration(new ItemConfiguration());
    }
}