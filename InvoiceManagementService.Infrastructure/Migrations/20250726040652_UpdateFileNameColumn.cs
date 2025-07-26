using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagementService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_name",
                table: "Invoices");
        }
    }
}
