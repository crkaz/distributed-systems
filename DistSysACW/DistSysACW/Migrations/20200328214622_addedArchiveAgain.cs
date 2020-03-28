using Microsoft.EntityFrameworkCore.Migrations;

namespace DistSysACW.Migrations
{
    public partial class addedArchiveAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArchiveApiKey",
                table: "Logs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArchivedLogs",
                columns: table => new
                {
                    ApiKey = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivedLogs", x => x.ApiKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ArchiveApiKey",
                table: "Logs",
                column: "ArchiveApiKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_ArchivedLogs_ArchiveApiKey",
                table: "Logs",
                column: "ArchiveApiKey",
                principalTable: "ArchivedLogs",
                principalColumn: "ApiKey",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_ArchivedLogs_ArchiveApiKey",
                table: "Logs");

            migrationBuilder.DropTable(
                name: "ArchivedLogs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_ArchiveApiKey",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ArchiveApiKey",
                table: "Logs");
        }
    }
}
