using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceTechnologyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DanhGia_ProductId",
                table: "DanhGia");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "DanhGia",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_ProductId",
                table: "DanhGia",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DanhGia_ProductId",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_ProductId",
                table: "DanhGia",
                column: "ProductId",
                unique: true);
        }
    }
}
