using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CoinbitBackend.Migrations
{
    public partial class banks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bank_id",
                table: "Customers",
                type: "integer",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "bankcardpic",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "card_number",
                table: "Customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "idcardpic",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "selfiepic",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sheba_number",
                table: "Customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_bank_id",
                table: "Customers",
                column: "bank_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_banks_bank_id",
                table: "Customers",
                column: "bank_id",
                principalTable: "banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_banks_bank_id",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropIndex(
                name: "IX_Customers_bank_id",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "bank_id",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "bankcardpic",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "card_number",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "idcardpic",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "selfiepic",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "sheba_number",
                table: "Customers");
        }
    }
}
