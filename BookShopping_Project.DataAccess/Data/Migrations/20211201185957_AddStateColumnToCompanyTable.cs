using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_project.DataAccess.Migrations
{
    public partial class AddStateColumnToCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Companies");
        }
    }
}
