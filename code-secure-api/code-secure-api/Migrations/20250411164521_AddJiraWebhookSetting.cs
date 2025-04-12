using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class AddJiraWebhookSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scanners_NormalizedName",
                table: "Scanners");

            migrationBuilder.AddColumn<string>(
                name: "JiraWebhookSetting",
                table: "AppSettings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceControls_NormalizedUrl_Type",
                table: "SourceControls",
                columns: new[] { "NormalizedUrl", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scanners_NormalizedName_Type",
                table: "Scanners",
                columns: new[] { "NormalizedName", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SourceControls_NormalizedUrl_Type",
                table: "SourceControls");

            migrationBuilder.DropIndex(
                name: "IX_Scanners_NormalizedName_Type",
                table: "Scanners");

            migrationBuilder.DropColumn(
                name: "JiraWebhookSetting",
                table: "AppSettings");

            migrationBuilder.CreateIndex(
                name: "IX_Scanners_NormalizedName",
                table: "Scanners",
                column: "NormalizedName",
                unique: true);
        }
    }
}
