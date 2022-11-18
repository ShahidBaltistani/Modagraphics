using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Site3ColumnsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpecialInstruction",
                table: "site",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ScopeOfWork",
                table: "site",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "OtherQuestionsComments",
                table: "site",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpecialInstruction",
                table: "site",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ScopeOfWork",
                table: "site",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OtherQuestionsComments",
                table: "site",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
