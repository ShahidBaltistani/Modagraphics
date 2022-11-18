using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddedPriceAndCommentsInBid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "bid",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "bid",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "bid");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "bid");
        }
    }
}
