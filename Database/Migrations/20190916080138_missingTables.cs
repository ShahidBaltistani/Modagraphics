using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class missingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Installer",
                table: "Installer");

            migrationBuilder.RenameTable(
                name: "Installer",
                newName: "installer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_installer",
                table: "installer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "crew",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    CityId = table.Column<uint>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    InstallerId = table.Column<uint>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_crew", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FleetOwnerId = table.Column<uint>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsPoolVehicle = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    StatusByFleetOwner = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "site",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FleetOwnerId = table.Column<uint>(nullable: false),
                    ProjectId = table.Column<uint>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CSR = table.Column<string>(nullable: false),
                    SalesMan = table.Column<string>(nullable: false),
                    IsForBid = table.Column<bool>(nullable: false),
                    BidDueDate = table.Column<DateTime>(nullable: true),
                    SiteStartDate = table.Column<DateTime>(nullable: false),
                    SiteEndDate = table.Column<DateTime>(nullable: false),
                    MaximumBidAmount = table.Column<float>(nullable: false),
                    SalePrice = table.Column<float>(nullable: false),
                    ScopeOfWork = table.Column<string>(nullable: false),
                    SpecialInstruction = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    CityId = table.Column<uint>(nullable: false),
                    PONo = table.Column<string>(nullable: true),
                    EMPSJobNo = table.Column<string>(nullable: true),
                    TakeDownStatus = table.Column<string>(nullable: false),
                    SiteStatus = table.Column<int>(nullable: false),
                    IndoorFacility = table.Column<bool>(nullable: false),
                    BusinessHours = table.Column<bool>(nullable: false),
                    WeekendHours = table.Column<bool>(nullable: false),
                    IsSpottingTractor = table.Column<bool>(nullable: false),
                    StoreEquipmentOnSite = table.Column<bool>(nullable: false),
                    SpotVehicle = table.Column<bool>(nullable: false),
                    RemoveExistingGraphics = table.Column<bool>(nullable: false),
                    YearsOnVehicle = table.Column<int>(nullable: true),
                    CleaningAndPrepVehicle = table.Column<bool>(nullable: false),
                    PowerWash = table.Column<bool>(nullable: false),
                    IsVinylGraphics = table.Column<bool>(nullable: false),
                    SealOfDecals = table.Column<bool>(nullable: false),
                    ArlonVinylFilmRemoval = table.Column<bool>(nullable: false),
                    ArnolVinylFilmApply = table.Column<bool>(nullable: false),
                    AveryVinylFilmRemoval = table.Column<bool>(nullable: false),
                    AveryVinylFilmApply = table.Column<bool>(nullable: false),
                    M3VinylFilmRemoval = table.Column<bool>(nullable: false),
                    M3VinylFilmApply = table.Column<bool>(nullable: false),
                    ArlonReflectiveFilmRemoval = table.Column<bool>(nullable: false),
                    ArlonReflectiveFilmApply = table.Column<bool>(nullable: false),
                    AveryReflectiveFilmRemoval = table.Column<bool>(nullable: false),
                    AveryReflectiveFilmApply = table.Column<bool>(nullable: false),
                    M3ReflectiveFilmRemoval = table.Column<bool>(nullable: false),
                    M3ReflectiveFilmApply = table.Column<bool>(nullable: false),
                    TotalSqFtRemoval = table.Column<float>(nullable: true),
                    TotalSqFtApply = table.Column<float>(nullable: true),
                    OtherQuestionsComments = table.Column<string>(nullable: false),
                    CustomerAvailabilityOnInstallationDay = table.Column<bool>(nullable: false),
                    InsideFacilities = table.Column<bool>(nullable: false),
                    IsDecalsReceived = table.Column<bool>(nullable: false),
                    VehicleCleanessBeforeArrival = table.Column<bool>(nullable: false),
                    IsInformed6FeetArea = table.Column<bool>(nullable: false),
                    VehicleAvailabilityDate = table.Column<DateTime>(nullable: false),
                    WorkHoursAtFacility = table.Column<string>(nullable: false),
                    AdditionalContacts = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_site", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "supervisor_site_association",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiteId = table.Column<uint>(nullable: false),
                    SupervisorId = table.Column<uint>(nullable: false),
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
                    table.PrimaryKey("PK_supervisor_site_association", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    PhoneNo = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    CityId = table.Column<uint>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_corporate",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    PhoneNo = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    CityId = table.Column<uint>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_user_corporate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_supervisor",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    PhoneNo = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    CityId = table.Column<uint>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_user_supervisor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_type_site_association",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VehicleTypeId = table.Column<uint>(nullable: false),
                    SiteId = table.Column<uint>(nullable: false),
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
                    table.PrimaryKey("PK_vehicle_type_site_association", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crew");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "site");

            migrationBuilder.DropTable(
                name: "supervisor_site_association");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "user_corporate");

            migrationBuilder.DropTable(
                name: "user_supervisor");

            migrationBuilder.DropTable(
                name: "vehicle_type_site_association");

            migrationBuilder.DropPrimaryKey(
                name: "PK_installer",
                table: "installer");

            migrationBuilder.RenameTable(
                name: "installer",
                newName: "Installer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Installer",
                table: "Installer",
                column: "Id");
        }
    }
}
