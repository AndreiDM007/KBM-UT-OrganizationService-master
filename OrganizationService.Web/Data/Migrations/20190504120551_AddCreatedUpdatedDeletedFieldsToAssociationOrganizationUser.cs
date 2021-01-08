using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class AddCreatedUpdatedDeletedFieldsToAssociationOrganizationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                table: "AssociatedOrganizationUser",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AssociatedOrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedAt",
                table: "AssociatedOrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AssociatedOrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                table: "AssociatedOrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AssociatedOrganizationUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AssociatedOrganizationUser");
        }
    }
}
