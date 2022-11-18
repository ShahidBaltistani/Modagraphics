using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class VehicleTypeSiteAssociationNewColumnsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "FrontHeight",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FrontWidth",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RearHeight",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RearWidth",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SideHeight",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SideWidth",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "VehiclePrice",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<uint>(
                name: "VehicleSpecificationId",
                table: "vehicle_type_site_association",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontHeight",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "FrontWidth",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "RearHeight",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "RearWidth",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "SideHeight",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "SideWidth",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "VehiclePrice",
                table: "vehicle_type_site_association");

            migrationBuilder.DropColumn(
                name: "VehicleSpecificationId",
                table: "vehicle_type_site_association");
        }
    }
}
