using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GroceryStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 6, scale: 2, nullable: false),
                    CurrentStock = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => new { x.ProductId, x.CartId });
                    table.ForeignKey(
                        name: "FK_Transactions_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CurrentStock", "Manufacturer", "Name", "Price" },
                values: new object[,]
                {
                    { 1225, "fresh-produce", 12, "Bosch", "1/2 in. Brushless Hammer Drill", 12.98m },
                    { 1709, "meat-seafood", 17, "Taylor & Partners Organic Meats", "Beef Choice Angus Ribeye Steak", 14.95m },
                    { 2177, "meat-seafood", 9, "CoscoProducts", "Cosco Three Step Steel Platform", 2.95m },
                    { 2585, "fresh-produce", 30, "Jack&Mary Organic Farms", "Green Cabbage Organic", 3.02m },
                    { 3674, "fresh-produce", 3, "DEWALT", "20V Max Cordless Drill Combo Kit", 10.96m },
                    { 4641, "coffee", 15, "Don Francisco", "Don Francisco Colombia Supremo Medium Roast", 9.76m },
                    { 4643, "coffee", 14, "Starbucks", "Starbucks Coffee Variety Pack, 100% Arabica", 40.91m },
                    { 4646, "coffee", 10, "Ethical Bean", "Ethical Bean Medium Dark Roast", 7.78m },
                    { 4875, "bread-bakery", 10, "Honda", "2800 Watt Inverter Generator", 47.45m },
                    { 5477, "dairy", 12, "Jack&Mary Organic Farms", "Cream Cheese", 2.95m },
                    { 5478, "dairy", 16, "Jack&Mary Organic Farms", "Low Fat Vanilla Yogurt", 1.95m },
                    { 5774, "candy", 1, "Hershey's", "HERSHEY'S, Milk Chocolate Almond", 1.45m },
                    { 5851, "fresh-produce", 0, "Jack&Mary Organic Farms", "Cucumber Organic", 0.95m },
                    { 6483, "candy", 2, "Cadbury", "Cadbury Milk Chocolate", 2.65m },
                    { 7395, "meat-seafood", 12, "Taylor & Partners Organic Meats", "Boneless Skinless Chicken Breasts", 7.45m },
                    { 8554, "candy", 3, "Ferrero", "Kinder Joy Eggs", 1.05m },
                    { 8739, "fresh-produce", 27, "Jack&Mary Organic Farms", "Fresh Spinach Organic", 2.95m },
                    { 8753, "dairy", 24, "Jack&Mary Organic Farms", "Reduced Fat Milk", 3.45m },
                    { 9482, "dairy", 16, "Jack&Mary Organic Farms", "Whole Milk", 3.55m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CartId",
                table: "Transactions",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
