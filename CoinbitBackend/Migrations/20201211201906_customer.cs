using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CoinbitBackend.Migrations
{
    public partial class customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createDate",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    fatherName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    gender = table.Column<bool>(type: "boolean", nullable: true),
                    birthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    nationalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mobile = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    company = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    postalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropColumn(
                name: "createDate",
                table: "Users");
        }
    }
}
