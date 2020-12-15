using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinbitBackend.Migrations
{
    public partial class coindataadd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Series",
                table: "CoinDatas");

            migrationBuilder.AddColumn<DateTime>(
                name: "SeriesDate",
                table: "CoinDatas",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeriesDate",
                table: "CoinDatas");

            migrationBuilder.AddColumn<long>(
                name: "Series",
                table: "CoinDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
