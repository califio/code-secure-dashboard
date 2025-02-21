using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectCommits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitHash",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "MergeRequestId",
                table: "Scans");

            migrationBuilder.AddColumn<string>(
                name: "CommitHash",
                table: "ProjectCommits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitTitle",
                table: "ProjectCommits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MergeRequestId",
                table: "ProjectCommits",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitHash",
                table: "ProjectCommits");

            migrationBuilder.DropColumn(
                name: "CommitTitle",
                table: "ProjectCommits");

            migrationBuilder.DropColumn(
                name: "MergeRequestId",
                table: "ProjectCommits");

            migrationBuilder.AddColumn<string>(
                name: "CommitHash",
                table: "Scans",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MergeRequestId",
                table: "Scans",
                type: "text",
                nullable: true);
        }
    }
}
