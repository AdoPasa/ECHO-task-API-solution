using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YahooFinanceAgregator.Migrations
{
    public partial class TickerTableExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FoundedYear",
                table: "Tickers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "MarketCap",
                table: "Tickers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfEmployees",
                table: "Tickers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "FoundedYear",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "MarketCap",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "NumberOfEmployees",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Tickers");
        }
    }
}
