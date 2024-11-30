using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceComparisonApp.Migrations
{
    /// <inheritdoc />
    public partial class prourl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProUrl",
                table: "Products");
        }
    }
}
