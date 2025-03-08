using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFixedAtFindingAndPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResolvedAt",
                table: "ScanProjectPackages",
                newName: "FixedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "FixedAt",
                table: "ScanFindings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FixedAt",
                table: "ProjectPackages",
                type: "timestamp with time zone",
                nullable: true);
            // remove finding (dependency) in findings
            migrationBuilder.Sql(@"
                DELETE FROM ""Findings"" f
                USING ""Scanners"" s
                WHERE f.""ScannerId"" = s.""Id""
                AND s.""Type"" = 3
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedAt",
                table: "ScanFindings");

            migrationBuilder.DropColumn(
                name: "FixedAt",
                table: "ProjectPackages");

            migrationBuilder.RenameColumn(
                name: "FixedAt",
                table: "ScanProjectPackages",
                newName: "ResolvedAt");
        }
    }
}
