using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RollbackedAt",
                table: "ProfileValue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "ProfileValue",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RollbackedAt",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Profile",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RollbackedAt",
                table: "OrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "OrganizationUser",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RollbackedAt",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Organization",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollbackedAt",
                table: "ProfileValue");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "ProfileValue");

            migrationBuilder.DropColumn(
                name: "RollbackedAt",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "RollbackedAt",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "RollbackedAt",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Organization");
        }
    }
}
