using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class FleetOwnerIdAddedInSupervisor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "FleetOwnerId",
                table: "supervisor",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FleetOwnerId",
                table: "supervisor");
        }
    }
}
