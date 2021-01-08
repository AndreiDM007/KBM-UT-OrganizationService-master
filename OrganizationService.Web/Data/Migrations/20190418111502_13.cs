using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                table: "OrganizationUser",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                table: "Organization",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrganizationUser");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Organization");
        }
    }
}
