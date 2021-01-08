using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class RemoveUniqueConstraintinAssociatedOrganizationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId1_OrganizationUserId2_AssociationType",
                table: "AssociatedOrganizationUser");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId1",
                table: "AssociatedOrganizationUser",
                column: "OrganizationUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId1",
                table: "AssociatedOrganizationUser");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId1_OrganizationUserId2_AssociationType",
                table: "AssociatedOrganizationUser",
                columns: new[] { "OrganizationUserId1", "OrganizationUserId2", "AssociationType" },
                unique: true);
        }
    }
}
