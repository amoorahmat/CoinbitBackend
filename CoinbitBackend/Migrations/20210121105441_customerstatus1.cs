using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinbitBackend.Migrations
{
    public partial class customerstatus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerStatuses",
                table: "CustomerStatuses");

            migrationBuilder.RenameTable(
                name: "CustomerStatuses",
                newName: "CustomerStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerStatus",
                table: "CustomerStatus",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerStatus",
                table: "CustomerStatus");

            migrationBuilder.RenameTable(
                name: "CustomerStatus",
                newName: "CustomerStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerStatuses",
                table: "CustomerStatuses",
                column: "Id");
        }
    }
}
