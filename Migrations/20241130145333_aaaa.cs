using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceComparisonApp.Migrations
{
    /// <inheritdoc />
    public partial class aaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProUrl",
                table: "Products",
                newName: "ProductUrl");

            migrationBuilder.AddColumn<int>(
                name: "SoldQty",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldQty",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductUrl",
                table: "Products",
                newName: "ProUrl");
        }
    }
}
