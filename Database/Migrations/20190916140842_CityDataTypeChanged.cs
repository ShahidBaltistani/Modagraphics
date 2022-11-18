using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CityDataTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCity",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "PCity",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "CCity",
                table: "fleet_owner");

            migrationBuilder.DropColumn(
                name: "PCity",
                table: "fleet_owner");

            migrationBuilder.AddColumn<uint>(
                name: "CCityId",
                table: "installer",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "PCityId",
                table: "installer",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "CCityId",
                table: "fleet_owner",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "PCityId",
                table: "fleet_owner",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCityId",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "PCityId",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "CCityId",
                table: "fleet_owner");

            migrationBuilder.DropColumn(
                name: "PCityId",
                table: "fleet_owner");

            migrationBuilder.AddColumn<string>(
                name: "CCity",
                table: "installer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PCity",
                table: "installer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCity",
                table: "fleet_owner",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PCity",
                table: "fleet_owner",
                nullable: false,
                defaultValue: "");
        }
    }
}
