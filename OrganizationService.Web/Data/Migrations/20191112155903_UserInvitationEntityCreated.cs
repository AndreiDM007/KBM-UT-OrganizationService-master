using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class UserInvitationEntityCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInvitation",
                columns: table => new
                {
                    UserInvitationEntityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    OrganizationUserId = table.Column<int>(nullable: false),
                    InviteGuid = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<long>(nullable: false),
                    AcceptedAt = table.Column<long>(nullable: true),
                    DeclinedAt = table.Column<long>(nullable: true),
                    DeletedAt = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvitation", x => x.UserInvitationEntityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_InviteGuid",
                table: "UserInvitation",
                column: "InviteGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInvitation");
        }
    }
}
