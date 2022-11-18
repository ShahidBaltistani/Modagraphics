using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class VehicleTypeSiteAssociationTableRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle_type_site_association");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "job",
                newName: "SiteVehicleTypeId");

            migrationBuilder.CreateTable(
                name: "site_vehicle_type",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VehicleTypeId = table.Column<uint>(nullable: false),
                    SiteId = table.Column<uint>(nullable: false),
                    VehicleSpecificationId = table.Column<uint>(nullable: false),
                    VehiclePrice = table.Column<float>(nullable: false),
                    SideHeight = table.Column<float>(nullable: false),
                    SideWidth = table.Column<float>(nullable: false),
                    FrontHeight = table.Column<float>(nullable: false),
                    FrontWidth = table.Column<float>(nullable: false),
                    RearHeight = table.Column<float>(nullable: false),
                    RearWidth = table.Column<float>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<int>(nullable: false),
                    TZOSModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_vehicle_type", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "site_vehicle_type");

            migrationBuilder.RenameColumn(
                name: "SiteVehicleTypeId",
                table: "job",
                newName: "VehicleTypeId");

            migrationBuilder.CreateTable(
                name: "vehicle_type_site_association",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FrontHeight = table.Column<float>(nullable: false),
                    FrontWidth = table.Column<float>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    RearHeight = table.Column<float>(nullable: false),
                    RearWidth = table.Column<float>(nullable: false),
                    SideHeight = table.Column<float>(nullable: false),
                    SideWidth = table.Column<float>(nullable: false),
                    SiteId = table.Column<uint>(nullable: false),
                    TZOSCreatedBy = table.Column<int>(nullable: false),
                    TZOSModifiedBy = table.Column<int>(nullable: true),
                    VehiclePrice = table.Column<float>(nullable: false),
                    VehicleSpecificationId = table.Column<uint>(nullable: false),
                    VehicleTypeId = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_type_site_association", x => x.Id);
                });
        }
    }
}
