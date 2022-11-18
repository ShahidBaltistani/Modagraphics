using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class DataTypeChangedForLPNnVIN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VIN",
                table: "job",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<string>(
                name: "LPN",
                table: "job",
                nullable: false,
                oldClrType: typeof(uint));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "VIN",
                table: "job",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<uint>(
                name: "LPN",
                table: "job",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
