using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class DataContextUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRole_OrganizationUserId",
                table: "OrganizationUserRole",
                column: "OrganizationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUserRole_OrganizationUser_OrganizationUserId",
                table: "OrganizationUserRole",
                column: "OrganizationUserId",
                principalTable: "OrganizationUser",
                principalColumn: "OrganizationUserEntityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUserRole_OrganizationUser_OrganizationUserId",
                table: "OrganizationUserRole");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUserRole_OrganizationUserId",
                table: "OrganizationUserRole");
        }
    }
}
