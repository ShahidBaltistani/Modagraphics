using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class crewimagesupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "CrewId",
                table: "job_images",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "SiteId",
                table: "job_images",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrewId",
                table: "job_images");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "job_images");
        }
    }
}
