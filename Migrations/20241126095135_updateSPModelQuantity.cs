using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceTechnologyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class updateSPModelQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "SanPham");
        }
    }
}
