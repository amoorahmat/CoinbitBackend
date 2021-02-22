using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinbitBackend.Migrations
{
    public partial class verifyusercus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_banks_bank_id",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "bank_id",
                table: "Customers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "verify_user_id",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_banks_bank_id",
                table: "Customers",
                column: "bank_id",
                principalTable: "banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_banks_bank_id",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "verify_user_id",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "bank_id",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_banks_bank_id",
                table: "Customers",
                column: "bank_id",
                principalTable: "banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
