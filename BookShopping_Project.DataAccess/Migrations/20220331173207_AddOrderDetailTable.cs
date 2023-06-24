using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_Project.DataAccess.Migrations
{
    public partial class AddOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Orderid = table.Column<int>(type: "int", nullable: false),
                    Productid = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_Orderid",
                        column: x => x.Orderid,
                        principalTable: "OrderHeaders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_Productid",
                        column: x => x.Productid,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_Orderid",
                table: "OrderDetails",
                column: "Orderid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_Productid",
                table: "OrderDetails",
                column: "Productid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");
        }
    }
}
