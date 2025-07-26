using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagementService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_id",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_id",
                table: "Invoices");
        }
    }
}
