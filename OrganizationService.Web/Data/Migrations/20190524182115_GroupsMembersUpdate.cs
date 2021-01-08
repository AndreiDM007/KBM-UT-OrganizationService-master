using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class GroupsMembersUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_OrganizationUser_GroupId",
                table: "GroupMember");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_OrganizationUserId",
                table: "GroupMember",
                column: "OrganizationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_OrganizationUser_OrganizationUserId",
                table: "GroupMember",
                column: "OrganizationUserId",
                principalTable: "OrganizationUser",
                principalColumn: "OrganizationUserEntityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_OrganizationUser_OrganizationUserId",
                table: "GroupMember");

            migrationBuilder.DropIndex(
                name: "IX_GroupMember_OrganizationUserId",
                table: "GroupMember");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_OrganizationUser_GroupId",
                table: "GroupMember",
                column: "GroupId",
                principalTable: "OrganizationUser",
                principalColumn: "OrganizationUserEntityId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
