using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinbitBackend.Migrations
{
    public partial class usercus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_user_id",
                table: "Customers",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_user_id",
                table: "Customers",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_user_id",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_user_id",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Customers");
        }
    }
}
