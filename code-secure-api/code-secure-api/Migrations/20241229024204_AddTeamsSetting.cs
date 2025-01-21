using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamsSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamsNotificationSetting",
                table: "AppSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamsNotificationSetting",
                table: "AppSettings");
        }
    }
}
