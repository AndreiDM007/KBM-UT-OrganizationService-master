using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class OrgUserAddedUserInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_OrganizationUserId",
                table: "UserInvitation",
                column: "OrganizationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_OrganizationUser_OrganizationUserId",
                table: "UserInvitation",
                column: "OrganizationUserId",
                principalTable: "OrganizationUser",
                principalColumn: "OrganizationUserEntityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_OrganizationUser_OrganizationUserId",
                table: "UserInvitation");

            migrationBuilder.DropIndex(
                name: "IX_UserInvitation_OrganizationUserId",
                table: "UserInvitation");
        }
    }
}
