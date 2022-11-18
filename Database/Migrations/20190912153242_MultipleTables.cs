using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class MultipleTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "TZOSCreatedBy",
                table: "vehicle_type",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "TZOSModifiedBy",
                table: "vehicle_type",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bid",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InstallerId = table.Column<uint>(nullable: false),
                    SiteId = table.Column<uint>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<uint>(nullable: false),
                    TZOSModifiedBy = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    StateId = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShortName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    StateId = table.Column<uint>(nullable: false),
                    PhoneCode = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fleet_owner",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    PhoneNo = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    PZipCode = table.Column<string>(nullable: false),
                    PCity = table.Column<string>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: false),
                    BillToAddress = table.Column<string>(nullable: false),
                    CZipCode = table.Column<string>(nullable: false),
                    CCity = table.Column<string>(nullable: false),
                    FaxNo = table.Column<string>(nullable: false),
                    CContactNo = table.Column<string>(nullable: false),
                    BIEmbededCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<uint>(nullable: false),
                    TZOSModifiedBy = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fleet_owner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Installer",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    PhoneNo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PZipCode = table.Column<string>(nullable: true),
                    PCity = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    TravelInMiles = table.Column<float>(nullable: false),
                    IsPreferred = table.Column<bool>(nullable: false),
                    CompanyName = table.Column<string>(nullable: false),
                    BillToAddress = table.Column<string>(nullable: true),
                    CZipCode = table.Column<string>(nullable: true),
                    CCity = table.Column<string>(nullable: true),
                    FaxNo = table.Column<string>(nullable: true),
                    CContactNo = table.Column<string>(nullable: true),
                    NDA = table.Column<string>(nullable: true),
                    Insurance = table.Column<string>(nullable: true),
                    InsuranceExpirayDate = table.Column<DateTime>(nullable: false),
                    W9Document = table.Column<string>(nullable: true),
                    CompanyEmail = table.Column<string>(nullable: true),
                    FEINumber = table.Column<string>(nullable: true),
                    CertificationTraining = table.Column<string>(nullable: true),
                    IsInstallersEmployee = table.Column<bool>(nullable: false),
                    IsEmployeesBackgroundCheck = table.Column<bool>(nullable: false),
                    IsDrugTest = table.Column<bool>(nullable: false),
                    EquipmentOwned = table.Column<string>(nullable: true),
                    InstallLocations = table.Column<string>(nullable: true),
                    InstallProjectCompleted = table.Column<string>(nullable: true),
                    YearInBusiness = table.Column<uint>(nullable: false),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<uint>(nullable: false),
                    TZOSModifiedBy = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VIN = table.Column<uint>(nullable: false),
                    LPN = table.Column<uint>(nullable: false),
                    UnitNo = table.Column<uint>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    VehicleTypeId = table.Column<uint>(nullable: false),
                    VehicleCondition = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<uint>(nullable: false),
                    TZOSModifiedBy = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job_images",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<uint>(nullable: false),
                    IsStartingPicture = table.Column<bool>(nullable: false),
                    Picture = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<uint>(nullable: false),
                    TZOSModifiedBy = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "login",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssociatedId = table.Column<uint>(nullable: false),
                    UserName = table.Column<uint>(nullable: false),
                    Password = table.Column<uint>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<uint>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<uint>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    TZOSCreatedBy = table.Column<uint>(nullable: false),
                    TZOSModifiedBy = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bid");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "fleet_owner");

            migrationBuilder.DropTable(
                name: "Installer");

            migrationBuilder.DropTable(
                name: "job");

            migrationBuilder.DropTable(
                name: "job_images");

            migrationBuilder.DropTable(
                name: "login");

            migrationBuilder.DropColumn(
                name: "TZOSCreatedBy",
                table: "vehicle_type");

            migrationBuilder.DropColumn(
                name: "TZOSModifiedBy",
                table: "vehicle_type");
        }
    }
}
