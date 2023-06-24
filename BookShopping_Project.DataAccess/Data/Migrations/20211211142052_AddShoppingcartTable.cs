using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_project.DataAccess.Migrations
{
    public partial class AddShoppingcartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shoppingcarts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Productid = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoppingcarts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shoppingcarts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shoppingcarts_Products_Productid",
                        column: x => x.Productid,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shoppingcarts_ApplicationUserId",
                table: "Shoppingcarts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppingcarts_Productid",
                table: "Shoppingcarts",
                column: "Productid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shoppingcarts");
        }
    }
}
