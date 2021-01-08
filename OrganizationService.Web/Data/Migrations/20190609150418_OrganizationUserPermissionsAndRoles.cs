using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class OrganizationUserPermissionsAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationUserPermission",
                columns: table => new
                {
                    OrganizationUserPermissionEntityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationUserId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Permissions = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<long>(nullable: false),
                    UpdatedAt = table.Column<long>(nullable: true),
                    DeletedAt = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUserPermission", x => x.OrganizationUserPermissionEntityId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUserRole",
                columns: table => new
                {
                    OrganizationUserRoleEntityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationUserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<long>(nullable: false),
                    UpdatedAt = table.Column<long>(nullable: true),
                    DeletedAt = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUserRole", x => x.OrganizationUserRoleEntityId);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RolePermissionEntityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: true),
                    Permissions = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<long>(nullable: false),
                    UpdatedAt = table.Column<long>(nullable: true),
                    DeletedAt = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.RolePermissionEntityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationUserPermission");

            migrationBuilder.DropTable(
                name: "OrganizationUserRole");

            migrationBuilder.DropTable(
                name: "RolePermission");
        }
    }
}
