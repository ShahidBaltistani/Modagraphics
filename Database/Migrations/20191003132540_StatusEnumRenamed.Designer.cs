﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(RapidusContext))]
    [Migration("20191003132540_StatusEnumRenamed")]
    partial class StatusEnumRenamed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Database.Models.Bid", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<uint>("InstallerId");

                    b.Property<bool>("IsDeleted");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<uint>("SiteId");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("bid");
                });

            modelBuilder.Entity("Database.Models.City", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<uint>("StateId");

                    b.HasKey("Id");

                    b.ToTable("city");
                });

            modelBuilder.Entity("Database.Models.CorporateUser", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<uint>("CityId");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PhoneNo")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<uint?>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("corporate_user");
                });

            modelBuilder.Entity("Database.Models.Country", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<uint>("PhoneCode");

                    b.Property<string>("ShortName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("country");
                });

            modelBuilder.Entity("Database.Models.Crew", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<uint>("CityId");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<uint>("InstallerId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PhoneNo")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<uint>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("crew");
                });

            modelBuilder.Entity("Database.Models.FleetOwner", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("BIEmbededCode");

                    b.Property<string>("BillToAddress")
                        .IsRequired();

                    b.Property<uint>("CCityId");

                    b.Property<string>("CContactNo")
                        .IsRequired();

                    b.Property<uint?>("CZipCode");

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FaxNo");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<uint>("PCityId");

                    b.Property<uint?>("PZipCode");

                    b.Property<string>("PhoneNo")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<uint>("UserCorporateId");

                    b.HasKey("Id");

                    b.ToTable("fleet_owner");
                });

            modelBuilder.Entity("Database.Models.Installer", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("BillToAddress");

                    b.Property<uint>("CCityId");

                    b.Property<string>("CContactNo");

                    b.Property<uint?>("CZipCode");

                    b.Property<string>("CertificationTraining");

                    b.Property<string>("CompanyEmail");

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("EquipmentOwned");

                    b.Property<string>("FEINumber");

                    b.Property<string>("FaxNo");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("InstallLocations");

                    b.Property<string>("InstallProjectCompleted");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsDrugTest");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<bool>("IsEmployeesBackgroundCheck");

                    b.Property<bool>("IsInstallersEmployee");

                    b.Property<bool>("IsPreferred");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<uint>("NoOfEmployees");

                    b.Property<uint>("PCityId");

                    b.Property<uint?>("PZipCode");

                    b.Property<string>("PhoneNo");

                    b.Property<string>("Picture");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<float>("TravelInMiles");

                    b.Property<uint>("YearInBusiness");

                    b.HasKey("Id");

                    b.ToTable("installer");
                });

            modelBuilder.Entity("Database.Models.Job", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LPN")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<uint>("SiteId");

                    b.Property<uint>("SiteVehicleTypeId");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<uint>("UnitNo");

                    b.Property<string>("VIN")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("job");
                });

            modelBuilder.Entity("Database.Models.JobImages", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsStartingPicture");

                    b.Property<uint>("JobId");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Picture")
                        .IsRequired();

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("job_images");
                });

            modelBuilder.Entity("Database.Models.Log", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EnteredOn");

                    b.Property<string>("IP")
                        .IsRequired();

                    b.Property<string>("UserAgent")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("log");
                });

            modelBuilder.Entity("Database.Models.Login", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("AssociatedId");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("Role");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("login");
                });

            modelBuilder.Entity("Database.Models.Project", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<uint>("FleetOwnerId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPoolVehicle");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<int>("StatusByFleetOwner");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("project");
                });

            modelBuilder.Entity("Database.Models.Site", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalContacts");

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<bool>("ArlonReflectiveFilmApply");

                    b.Property<bool>("ArlonReflectiveFilmRemoval");

                    b.Property<bool>("ArlonVinylFilmApply");

                    b.Property<bool>("ArlonVinylFilmRemoval");

                    b.Property<bool>("AveryReflectiveFilmApply");

                    b.Property<bool>("AveryReflectiveFilmRemoval");

                    b.Property<bool>("AveryVinylFilmApply");

                    b.Property<bool>("AveryVinylFilmRemoval");

                    b.Property<DateTime?>("BidDueDate");

                    b.Property<bool>("BusinessHours");

                    b.Property<string>("CSR")
                        .IsRequired();

                    b.Property<uint>("CityId");

                    b.Property<bool>("CleaningAndPrepVehicle");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("CustomerAvailabilityOnInstallationDay");

                    b.Property<string>("EPMSJobNo");

                    b.Property<uint>("FleetOwnerId");

                    b.Property<bool>("IndoorFacility");

                    b.Property<bool>("InsideFacilities");

                    b.Property<bool>("IsDecalsReceived");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsForBid");

                    b.Property<bool>("IsInformed6FeetArea");

                    b.Property<bool>("IsSpottingTractor");

                    b.Property<bool>("IsVinylGraphics");

                    b.Property<bool>("M3ReflectiveFilmApply");

                    b.Property<bool>("M3ReflectiveFilmRemoval");

                    b.Property<bool>("M3VinylFilmApply");

                    b.Property<bool>("M3VinylFilmRemoval");

                    b.Property<float>("MaximumBidAmount");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("OtherQuestionsComments")
                        .IsRequired();

                    b.Property<string>("PONo");

                    b.Property<bool>("PowerWash");

                    b.Property<uint>("ProjectId");

                    b.Property<bool>("RemoveExistingGraphics");

                    b.Property<float>("SalePrice");

                    b.Property<string>("SalesMan")
                        .IsRequired();

                    b.Property<string>("ScopeOfWork")
                        .IsRequired();

                    b.Property<bool>("SealOfDecals");

                    b.Property<DateTime>("SiteEndDate");

                    b.Property<DateTime>("SiteStartDate");

                    b.Property<int>("SiteStatus");

                    b.Property<string>("SpecialInstruction")
                        .IsRequired();

                    b.Property<bool>("SpotVehicle");

                    b.Property<bool>("StoreEquipmentOnSite");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<int>("TakeDownStatus");

                    b.Property<float?>("TotalSqFtApply");

                    b.Property<float?>("TotalSqFtRemoval");

                    b.Property<DateTime>("VehicleAvailabilityDate");

                    b.Property<bool>("VehicleCleanessBeforeArrival");

                    b.Property<bool>("WeekendHours");

                    b.Property<string>("WorkHoursAtFacility")
                        .IsRequired();

                    b.Property<int?>("YearsOnVehicle");

                    b.Property<uint>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("site");
                });

            modelBuilder.Entity("Database.Models.SiteVehicleType", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<float>("FrontHeight");

                    b.Property<float>("FrontWidth");

                    b.Property<bool>("IsDeleted");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<float>("RearHeight");

                    b.Property<float>("RearWidth");

                    b.Property<float>("SideHeight");

                    b.Property<float>("SideWidth");

                    b.Property<uint>("SiteId");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<float>("VehiclePrice");

                    b.Property<uint>("VehicleSpecificationId");

                    b.Property<uint>("VehicleTypeId");

                    b.HasKey("Id");

                    b.ToTable("site_vehicle_type");
                });

            modelBuilder.Entity("Database.Models.State", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CountryId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("state");
                });

            modelBuilder.Entity("Database.Models.Supervisor", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<uint>("CityId");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<uint>("FleetOwnerId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PhoneNo")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<uint>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("supervisor");
                });

            modelBuilder.Entity("Database.Models.SupervisorSiteAssociation", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<uint>("SiteId");

                    b.Property<uint>("SupervisorId");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("supervisor_site_association");
                });

            modelBuilder.Entity("Database.Models.User", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<uint>("CityId");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PhoneNo")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<int>("Status");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<int>("UserType");

                    b.Property<uint?>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Database.Models.VehicleType", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("BasePrice");

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Image");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("vehicle_type");
                });

            modelBuilder.Entity("Database.Models.VehicleTypeSpecifications", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<float>("Value");

                    b.Property<uint>("VehicleTypeId");

                    b.HasKey("Id");

                    b.ToTable("vehicle_type_specs");
                });
#pragma warning restore 612, 618
        }
    }
}
