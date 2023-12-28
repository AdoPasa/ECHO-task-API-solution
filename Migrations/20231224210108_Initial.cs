using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YahooFinanceAgregator.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TickerHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpenPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TickerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TickerHistory_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TickerHistory_TickerId",
                table: "TickerHistory",
                column: "TickerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerHistory");

            migrationBuilder.DropTable(
                name: "Tickers");
        }
    }
}
