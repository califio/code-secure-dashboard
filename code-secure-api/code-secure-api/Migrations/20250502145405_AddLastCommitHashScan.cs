using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class AddLastCommitHashScan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastCommitHash",
                table: "Scans",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCommitHash",
                table: "Scans");
        }
    }
}
