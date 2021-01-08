using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileValueId",
                table: "ProfileValue",
                newName: "ProfileValueEntityId");

            migrationBuilder.RenameColumn(
                name: "ProfileParameterId",
                table: "ProfileParameter",
                newName: "ProfileParameterEntityId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Profile",
                newName: "ProfileEntityId");

            migrationBuilder.RenameColumn(
                name: "OrganizationUserId",
                table: "OrganizationUser",
                newName: "OrganizationUserEntityId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Organization",
                newName: "OrganizationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId2",
                table: "AssociatedOrganizationUser",
                column: "OrganizationUserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedOrganizationUser_OrganizationUser_OrganizationUserId2",
                table: "AssociatedOrganizationUser",
                column: "OrganizationUserId2",
                principalTable: "OrganizationUser",
                principalColumn: "OrganizationUserEntityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedOrganizationUser_OrganizationUser_OrganizationUserId2",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId2",
                table: "AssociatedOrganizationUser");

            migrationBuilder.RenameColumn(
                name: "ProfileValueEntityId",
                table: "ProfileValue",
                newName: "ProfileValueId");

            migrationBuilder.RenameColumn(
                name: "ProfileParameterEntityId",
                table: "ProfileParameter",
                newName: "ProfileParameterId");

            migrationBuilder.RenameColumn(
                name: "ProfileEntityId",
                table: "Profile",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "OrganizationUserEntityId",
                table: "OrganizationUser",
                newName: "OrganizationUserId");

            migrationBuilder.RenameColumn(
                name: "OrganizationEntityId",
                table: "Organization",
                newName: "OrganizationId");
        }
    }
}
