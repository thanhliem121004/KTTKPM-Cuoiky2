using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceTechnologyWebsite.Migrations
{
    public partial class UpdateIdInttoLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa chính hiện tại
            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham");

            // Thay đổi kiểu dữ liệu của cột Id
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "SanPham",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            // Tạo lại khóa chính với kiểu dữ liệu mới
            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa chính
            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham");

            // Thay đổi kiểu dữ liệu của cột Id trở lại int
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SanPham",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            // Tạo lại khóa chính với kiểu dữ liệu int
            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham",
                column: "Id");
        }
    }
}