using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddFieldsForProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "project");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "project");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "project");
        }
    }
}
