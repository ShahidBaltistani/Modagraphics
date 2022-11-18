using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class crewimagesuploadchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "job_images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "job_images");
        }
    }
}
