using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPackages",
                table: "ProjectPackages");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProjectPackages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
            // Cập nhật các bản ghi hiện có với Guid ngẫu nhiên
            migrationBuilder.Sql(
                "UPDATE \"ProjectPackages\" SET \"Id\" = gen_random_uuid() WHERE \"Id\" = '00000000-0000-0000-0000-000000000000';"
            ); 
            
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectPackages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);
            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "ProjectPackages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProjectPackages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPackages",
                table: "ProjectPackages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPackages_ProjectId_PackageId_Location",
                table: "ProjectPackages",
                columns: new[] { "ProjectId", "PackageId", "Location" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPackages",
                table: "ProjectPackages");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPackages_ProjectId_PackageId_Location",
                table: "ProjectPackages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProjectPackages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectPackages");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "ProjectPackages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProjectPackages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPackages",
                table: "ProjectPackages",
                columns: new[] { "ProjectId", "PackageId", "Location" });
        }
    }
}
