using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class LoginDataTypesCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "login",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "login",
                nullable: false,
                oldClrType: typeof(uint));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "UserName",
                table: "login",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "Password",
                table: "login",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
