using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class MulitpleTables_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "SiteId",
                table: "job",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "job");
        }
    }
}
