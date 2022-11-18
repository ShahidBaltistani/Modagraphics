using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class DocsRemovedInstaller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "InsuranceExpirayDate",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "NDA",
                table: "installer");

            migrationBuilder.DropColumn(
                name: "W9Document",
                table: "installer");

            migrationBuilder.AlterColumn<uint>(
                name: "PZipCode",
                table: "installer",
                nullable: true,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<uint>(
                name: "CZipCode",
                table: "installer",
                nullable: true,
                oldClrType: typeof(uint));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "PZipCode",
                table: "installer",
                nullable: false,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "CZipCode",
                table: "installer",
                nullable: false,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Insurance",
                table: "installer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceExpirayDate",
                table: "installer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NDA",
                table: "installer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "W9Document",
                table: "installer",
                nullable: true);
        }
    }
}
