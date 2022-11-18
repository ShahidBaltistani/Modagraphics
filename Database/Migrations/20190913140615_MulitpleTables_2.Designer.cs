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
    [Migration("20190913140615_MulitpleTables_2")]
    partial class MulitpleTables_2
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

                    b.Property<string>("Status")
                        .IsRequired();

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

            modelBuilder.Entity("Database.Models.FleetOwner", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("BIEmbededCode");

                    b.Property<string>("BillToAddress")
                        .IsRequired();

                    b.Property<string>("CCity")
                        .IsRequired();

                    b.Property<string>("CContactNo")
                        .IsRequired();

                    b.Property<string>("CZipCode")
                        .IsRequired();

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FaxNo")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PCity")
                        .IsRequired();

                    b.Property<string>("PZipCode")
                        .IsRequired();

                    b.Property<string>("PhoneNo")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.HasKey("Id");

                    b.ToTable("fleet_owner");
                });

            modelBuilder.Entity("Database.Models.Installer", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("BillToAddress");

                    b.Property<string>("CCity");

                    b.Property<string>("CContactNo");

                    b.Property<string>("CZipCode");

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

                    b.Property<string>("Insurance");

                    b.Property<DateTime>("InsuranceExpirayDate");

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

                    b.Property<string>("NDA");

                    b.Property<string>("PCity");

                    b.Property<string>("PZipCode");

                    b.Property<string>("PhoneNo");

                    b.Property<string>("Picture");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<float>("TravelInMiles");

                    b.Property<string>("W9Document");

                    b.Property<uint>("YearInBusiness");

                    b.HasKey("Id");

                    b.ToTable("Installer");
                });

            modelBuilder.Entity("Database.Models.Job", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<uint>("LPN");

                    b.Property<uint?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<uint>("SiteId");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<int>("TZOSCreatedBy");

                    b.Property<int?>("TZOSModifiedBy");

                    b.Property<uint>("UnitNo");

                    b.Property<uint>("VIN");

                    b.Property<string>("VehicleCondition")
                        .IsRequired();

                    b.Property<uint>("VehicleTypeId");

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
