using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_Mall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeorderschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderID",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "OrderItems",
                newName: "VendorOrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                newName: "IX_OrderItems_VendorOrderID");

            migrationBuilder.CreateTable(
                name: "CustomerOrders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_AspNetUsers_ClientID",
                        column: x => x.ClientID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorOrders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingStreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerOrderID = table.Column<int>(type: "int", nullable: false),
                    StoreID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VendorOrders_CustomerOrders_CustomerOrderID",
                        column: x => x.CustomerOrderID,
                        principalTable: "CustomerOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorOrders_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_ClientID",
                table: "CustomerOrders",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrders_CustomerOrderID",
                table: "VendorOrders",
                column: "CustomerOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrders_StoreID",
                table: "VendorOrders",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_VendorOrders_VendorOrderID",
                table: "OrderItems",
                column: "VendorOrderID",
                principalTable: "VendorOrders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_VendorOrders_VendorOrderID",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "VendorOrders");

            migrationBuilder.DropTable(
                name: "CustomerOrders");

            migrationBuilder.RenameColumn(
                name: "VendorOrderID",
                table: "OrderItems",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_VendorOrderID",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderID");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingStreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ClientID",
                        column: x => x.ClientID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientID",
                table: "Orders",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderID",
                table: "OrderItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
