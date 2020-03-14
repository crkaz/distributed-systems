using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistSysLab7.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressIdentifier = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    House_Name_or_Number = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    AccountIdentifier = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.AccountIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(nullable: true),
                    Last_Name = table.Column<string>(nullable: true),
                    Date_of_Birth = table.Column<DateTime>(nullable: false),
                    AddressIdentifier = table.Column<int>(nullable: true),
                    BankAccountAccountIdentifier = table.Column<int>(nullable: true),
                    Age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_People_Addresses_AddressIdentifier",
                        column: x => x.AddressIdentifier,
                        principalTable: "Addresses",
                        principalColumn: "AddressIdentifier",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_BankAccounts_BankAccountAccountIdentifier",
                        column: x => x.BankAccountAccountIdentifier,
                        principalTable: "BankAccounts",
                        principalColumn: "AccountIdentifier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_AddressIdentifier",
                table: "People",
                column: "AddressIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_People_BankAccountAccountIdentifier",
                table: "People",
                column: "BankAccountAccountIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
