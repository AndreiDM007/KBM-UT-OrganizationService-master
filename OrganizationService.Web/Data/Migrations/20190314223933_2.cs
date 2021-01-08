using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "OrganizationUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "ProfileParameter",
                columns: new[] { "ProfileParameterId", "Name" },
                values: new object[] { 32, "PatientId" });

            migrationBuilder.InsertData(
                table: "ProfileParameter",
                columns: new[] { "ProfileParameterId", "Name" },
                values: new object[] { 33, "PatientId2" });

            migrationBuilder.InsertData(
                table: "ProfileParameter",
                columns: new[] { "ProfileParameterId", "Name" },
                values: new object[] { 34, "DeviceEditPermission" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 34);

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "OrganizationUser");
        }
    }
}
