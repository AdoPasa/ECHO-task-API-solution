using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YahooFinanceAgregator.Migrations
{
    public partial class MarketTimezoneAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarketTimeZone",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketTimeZone",
                table: "Tickers");
        }
    }
}
