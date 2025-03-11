using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSecure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexFinding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // remove finding not exists in ScanFinding
            migrationBuilder.Sql($@"DELETE FROM ""Findings"" f
            WHERE NOT EXISTS (
                SELECT 1
                FROM ""ScanFindings"" sf
                WHERE sf.""FindingId"" = f.""Id""
            );");
            // remove duplicate finding
            migrationBuilder.Sql(@"DELETE FROM ""Findings"" f
            WHERE f.""Id"" NOT IN (
                SELECT ""Id""
                FROM (
                         SELECT ""Id"",
                             ROW_NUMBER() OVER (PARTITION BY ""Identity"", ""ProjectId"" ORDER BY ""CreatedAt"" DESC) as rn
                         FROM ""Findings""
                     ) t
                WHERE rn = 1
            );");
            migrationBuilder.CreateIndex(
                name: "IX_Findings_Identity_ProjectId",
                table: "Findings",
                columns: new[] { "Identity", "ProjectId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Findings_Identity_ProjectId",
                table: "Findings");
        }
    }
}
