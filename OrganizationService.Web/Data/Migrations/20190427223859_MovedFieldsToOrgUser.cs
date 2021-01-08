using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class MovedFieldsToOrgUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrganizationUser",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "OrganizationUser",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrganizationUser",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "OrganizationUser",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                table: "OrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "OrganizationUser",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "OrganizationUser");
        }
    }
}
