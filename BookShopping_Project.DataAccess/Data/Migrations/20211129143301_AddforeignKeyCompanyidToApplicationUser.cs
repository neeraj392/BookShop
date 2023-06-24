using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_project.DataAccess.Migrations
{
    public partial class AddforeignKeyCompanyidToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Companyid",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Companyid",
                table: "AspNetUsers",
                column: "Companyid");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_Companyid",
                table: "AspNetUsers",
                column: "Companyid",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_Companyid",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Companyid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Companyid",
                table: "AspNetUsers");
        }
    }
}
