using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class ChangedTZOSDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "vehicle_type",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "vehicle_type",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "login",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "login",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "job_images",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "job_images",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "job",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "job",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "Installer",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "Installer",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "fleet_owner",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "fleet_owner",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<int>(
                name: "TZOSModifiedBy",
                table: "bid",
                nullable: true,
                oldClrType: typeof(uint),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TZOSCreatedBy",
                table: "bid",
                nullable: false,
                oldClrType: typeof(uint));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "vehicle_type",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "vehicle_type",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "login",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "login",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "job_images",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "job_images",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "job",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "job",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "Installer",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "Installer",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "fleet_owner",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "fleet_owner",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSModifiedBy",
                table: "bid",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "TZOSCreatedBy",
                table: "bid",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
