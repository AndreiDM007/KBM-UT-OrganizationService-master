using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProfileParameter",
                columns: new[] { "ProfileParameterId", "Name" },
                values: new object[,]
                {
                    { 1, "First Name" },
                    { 29, "Group" },
                    { 28, "Primary Care Physician" },
                    { 27, "Physician" },
                    { 26, "Generic 5" },
                    { 25, "Generic 4" },
                    { 24, "Generic 3" },
                    { 23, "Generic 2" },
                    { 22, "Generic 1" },
                    { 21, "Phone 4" },
                    { 20, "Phone 4" },
                    { 19, "Phone 3" },
                    { 18, "Phone 2" },
                    { 17, "Phone 1" },
                    { 30, "Sleep Doctor" },
                    { 16, "Email" },
                    { 14, "Country" },
                    { 13, "State" },
                    { 12, "City" },
                    { 11, "Address 2" },
                    { 10, "Address 1" },
                    { 9, "Date Collected" },
                    { 8, "Therapy Start Date" },
                    { 7, "BMI" },
                    { 6, "Weight" },
                    { 5, "Height" },
                    { 4, "Gender" },
                    { 3, "Birthdate" },
                    { 2, "Last Name" },
                    { 15, "Postal Code" },
                    { 31, "Practice" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProfileParameter",
                keyColumn: "ProfileParameterId",
                keyValue: 31);
        }
    }
}
