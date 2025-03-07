using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScanProjectPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScanProjectPackages_ScanId",
                table: "ScanProjectPackages");

            migrationBuilder.AddColumn<string>(
                name: "IgnoredReason",
                table: "ScanProjectPackages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "ScanProjectPackages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ScanProjectPackages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                table: "ScanProjectPackages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScanProjectPackages_ScanId_ProjectPackageId",
                table: "ScanProjectPackages",
                columns: new[] { "ScanId", "ProjectPackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScanProjectPackages_UpdatedById",
                table: "ScanProjectPackages",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ScanProjectPackages_Users_UpdatedById",
                table: "ScanProjectPackages",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScanProjectPackages_Users_UpdatedById",
                table: "ScanProjectPackages");

            migrationBuilder.DropIndex(
                name: "IX_ScanProjectPackages_ScanId_ProjectPackageId",
                table: "ScanProjectPackages");

            migrationBuilder.DropIndex(
                name: "IX_ScanProjectPackages_UpdatedById",
                table: "ScanProjectPackages");

            migrationBuilder.DropColumn(
                name: "IgnoredReason",
                table: "ScanProjectPackages");

            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "ScanProjectPackages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScanProjectPackages");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "ScanProjectPackages");

            migrationBuilder.CreateIndex(
                name: "IX_ScanProjectPackages_ScanId",
                table: "ScanProjectPackages",
                column: "ScanId");
        }
    }
}
