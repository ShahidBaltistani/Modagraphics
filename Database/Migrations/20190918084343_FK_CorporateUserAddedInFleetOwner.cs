using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class FK_CorporateUserAddedInFleetOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "UserCorporateId",
                table: "fleet_owner",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCorporateId",
                table: "fleet_owner");
        }
    }
}
