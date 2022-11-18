using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SiteTableColumnsRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EMPSJobNo",
                table: "site",
                newName: "EPMSJobNo");

            migrationBuilder.RenameColumn(
                name: "ArnolVinylFilmApply",
                table: "site",
                newName: "ArlonVinylFilmApply");

            migrationBuilder.AlterColumn<int>(
                name: "TakeDownStatus",
                table: "site",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EPMSJobNo",
                table: "site",
                newName: "EMPSJobNo");

            migrationBuilder.RenameColumn(
                name: "ArlonVinylFilmApply",
                table: "site",
                newName: "ArnolVinylFilmApply");

            migrationBuilder.AlterColumn<string>(
                name: "TakeDownStatus",
                table: "site",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
