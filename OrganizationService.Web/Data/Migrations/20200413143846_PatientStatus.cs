using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class PatientStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProfileParameter",
                columns: new[] { "ProfileParameterEntityId", "Name" },
                values: new object[] { 37, "PatientStatus" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterEntityId",
                keyValue: 37);
        }
    }
}
