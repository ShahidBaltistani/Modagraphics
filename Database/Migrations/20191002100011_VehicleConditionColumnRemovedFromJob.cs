using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class VehicleConditionColumnRemovedFromJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleCondition",
                table: "job");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleCondition",
                table: "job",
                nullable: false,
                defaultValue: "");
        }
    }
}
