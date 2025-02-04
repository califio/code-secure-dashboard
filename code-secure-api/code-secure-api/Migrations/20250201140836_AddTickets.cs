using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class AddTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "ProjectSettings");

            migrationBuilder.RenameColumn(
                name: "TeamsNotificationSetting",
                table: "AppSettings",
                newName: "TeamsSetting");

            migrationBuilder.AddColumn<string>(
                name: "JiraSetting",
                table: "ProjectSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SastSetting",
                table: "ProjectSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScaSetting",
                table: "ProjectSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamsSetting",
                table: "ProjectSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "ProjectPackages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "Findings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JiraSetting",
                table: "AppSettings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPackages_TicketId",
                table: "ProjectPackages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_TicketId",
                table: "Findings",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Findings_Tickets_TicketId",
                table: "Findings",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPackages_Tickets_TicketId",
                table: "ProjectPackages",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Findings_Tickets_TicketId",
                table: "Findings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPackages_Tickets_TicketId",
                table: "ProjectPackages");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPackages_TicketId",
                table: "ProjectPackages");

            migrationBuilder.DropIndex(
                name: "IX_Findings_TicketId",
                table: "Findings");

            migrationBuilder.DropColumn(
                name: "JiraSetting",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "SastSetting",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "ScaSetting",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "TeamsSetting",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "ProjectPackages");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Findings");

            migrationBuilder.DropColumn(
                name: "JiraSetting",
                table: "AppSettings");

            migrationBuilder.RenameColumn(
                name: "TeamsSetting",
                table: "AppSettings",
                newName: "TeamsNotificationSetting");

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "ProjectSettings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
