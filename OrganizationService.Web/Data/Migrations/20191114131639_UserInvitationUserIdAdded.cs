using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class UserInvitationUserIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InviteGuid",
                table: "UserInvitation",
                newName: "InvitationGuid");

            migrationBuilder.RenameIndex(
                name: "IX_UserInvitation_InviteGuid",
                table: "UserInvitation",
                newName: "IX_UserInvitation_InvitationGuid");

            migrationBuilder.AddColumn<string>(
                name: "ExternalUserId",
                table: "UserInvitation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "UserInvitation");

            migrationBuilder.RenameColumn(
                name: "InvitationGuid",
                table: "UserInvitation",
                newName: "InviteGuid");

            migrationBuilder.RenameIndex(
                name: "IX_UserInvitation_InvitationGuid",
                table: "UserInvitation",
                newName: "IX_UserInvitation_InviteGuid");
        }
    }
}
