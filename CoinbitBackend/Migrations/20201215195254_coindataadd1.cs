using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinbitBackend.Migrations
{
    public partial class coindataadd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Series",
                table: "CoinDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Series",
                table: "CoinDatas");
        }
    }
}
