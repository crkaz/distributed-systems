using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkLab.Migrations
{
    public partial class timestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "People",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "BankAccounts",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Addresses",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "People");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Addresses");
        }
    }
}
