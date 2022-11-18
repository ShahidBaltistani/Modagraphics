using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class StatusRejectedJobsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jobStatusLogs",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<uint>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    TZOSCreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobStatusLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rejectedJob",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<uint>(nullable: false),
                    Comments = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    TZOSCreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rejectedJob", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jobStatusLogs");

            migrationBuilder.DropTable(
                name: "rejectedJob");
        }
    }
}
