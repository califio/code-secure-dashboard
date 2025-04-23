using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class AddRedmineIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedmineSetting",
                table: "ProjectSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedmineSetting",
                table: "AppSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedmineSetting",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "RedmineSetting",
                table: "AppSettings");
        }
    }
}
