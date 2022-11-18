using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class NoOfEmpAddedInInstaller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "NoOfEmployees",
                table: "installer",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfEmployees",
                table: "installer");
        }
    }
}
