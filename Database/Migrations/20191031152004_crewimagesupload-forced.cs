using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class crewimagesuploadforced : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "job_images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "SiteId",
                table: "job_images",
                nullable: false,
                defaultValue: 0u);
        }
    }
}
