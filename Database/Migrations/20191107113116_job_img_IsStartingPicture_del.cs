using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class job_img_IsStartingPicture_del : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStartingPicture",
                table: "job_images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStartingPicture",
                table: "job_images",
                nullable: false,
                defaultValue: false);
        }
    }
}
