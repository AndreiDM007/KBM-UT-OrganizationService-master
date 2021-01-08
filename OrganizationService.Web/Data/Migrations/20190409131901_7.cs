using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RollbackedAt",
                table: "AssociatedOrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "AssociatedOrganizationUser",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollbackedAt",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "AssociatedOrganizationUser");
        }
    }
}
