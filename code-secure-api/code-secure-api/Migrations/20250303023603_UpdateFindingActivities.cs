using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFindingActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommitId",
                table: "FindingActivities",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewState",
                table: "FindingActivities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldState",
                table: "FindingActivities",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingActivities_CommitId",
                table: "FindingActivities",
                column: "CommitId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingActivities_ProjectCommits_CommitId",
                table: "FindingActivities",
                column: "CommitId",
                principalTable: "ProjectCommits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingActivities_ProjectCommits_CommitId",
                table: "FindingActivities");

            migrationBuilder.DropIndex(
                name: "IX_FindingActivities_CommitId",
                table: "FindingActivities");

            migrationBuilder.DropColumn(
                name: "CommitId",
                table: "FindingActivities");

            migrationBuilder.DropColumn(
                name: "NewState",
                table: "FindingActivities");

            migrationBuilder.DropColumn(
                name: "OldState",
                table: "FindingActivities");
        }
    }
}
