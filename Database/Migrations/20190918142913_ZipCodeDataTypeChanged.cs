using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class ZipCodeDataTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "ZipCode",
                table: "user_supervisor",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "ZipCode",
                table: "site",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "PZipCode",
                table: "installer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "CZipCode",
                table: "installer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "ZipCode",
                table: "crew",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "user_supervisor",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "site",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<string>(
                name: "PZipCode",
                table: "installer",
                nullable: true,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<string>(
                name: "CZipCode",
                table: "installer",
                nullable: true,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "crew",
                nullable: false,
                oldClrType: typeof(uint));
        }
    }
}
