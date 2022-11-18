using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SiteBidStatusLog_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "site_bid_status_log");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "site_bid_status_log");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "site_bid_status_log");

            migrationBuilder.DropColumn(
                name: "TZOSModifiedBy",
                table: "site_bid_status_log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "site_bid_status_log",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<uint>(
                name: "ModifiedBy",
                table: "site_bid_status_log",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "site_bid_status_log",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TZOSModifiedBy",
                table: "site_bid_status_log",
                nullable: true);
        }
    }
}
