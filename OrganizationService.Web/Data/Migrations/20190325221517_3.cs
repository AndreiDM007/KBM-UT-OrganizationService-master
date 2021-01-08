using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssociatedOrganizationUser",
                columns: table => new
                {
                    AssociatedOrganizationUserEntityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationUserId1 = table.Column<int>(nullable: false),
                    OrganizationUserId2 = table.Column<int>(nullable: false),
                    AssociationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociatedOrganizationUser", x => x.AssociatedOrganizationUserEntityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedOrganizationUser_OrganizationUserId1_OrganizationUserId2_AssociationType",
                table: "AssociatedOrganizationUser",
                columns: new[] { "OrganizationUserId1", "OrganizationUserId2", "AssociationType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociatedOrganizationUser");
        }
    }
}
