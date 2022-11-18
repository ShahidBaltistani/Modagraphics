using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class TableNameAndZipTypeChnaged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_supervisor",
                table: "user_supervisor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_corporate",
                table: "user_corporate");

            migrationBuilder.RenameTable(
                name: "user_supervisor",
                newName: "supervisor");

            migrationBuilder.RenameTable(
                name: "user_corporate",
                newName: "corporate_user");

            migrationBuilder.AlterColumn<uint>(
                name: "ZipCode",
                table: "user",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "PZipCode",
                table: "fleet_owner",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FaxNo",
                table: "fleet_owner",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "CZipCode",
                table: "fleet_owner",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "ZipCode",
                table: "corporate_user",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_supervisor",
                table: "supervisor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_corporate_user",
                table: "corporate_user",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_supervisor",
                table: "supervisor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_corporate_user",
                table: "corporate_user");

            migrationBuilder.RenameTable(
                name: "supervisor",
                newName: "user_supervisor");

            migrationBuilder.RenameTable(
                name: "corporate_user",
                newName: "user_corporate");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "user",
                nullable: false,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PZipCode",
                table: "fleet_owner",
                nullable: false,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FaxNo",
                table: "fleet_owner",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CZipCode",
                table: "fleet_owner",
                nullable: false,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "user_corporate",
                nullable: false,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_supervisor",
                table: "user_supervisor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_corporate",
                table: "user_corporate",
                column: "Id");
        }
    }
}
