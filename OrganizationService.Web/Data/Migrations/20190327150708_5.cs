using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedOrganizationUser_OrganizationUser_OrganizationUserId2",
                table: "AssociatedOrganizationUser");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId2",
                table: "AssociatedOrganizationUser");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedOrganizationUser_OrganizationUser_OrganizationUserId1",
                table: "AssociatedOrganizationUser",
                column: "OrganizationUserId1",
                principalTable: "OrganizationUser",
                principalColumn: "OrganizationUserEntityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedOrganizationUser_OrganizationUser_OrganizationUserId1",
                table: "AssociatedOrganizationUser");

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
    }
}
