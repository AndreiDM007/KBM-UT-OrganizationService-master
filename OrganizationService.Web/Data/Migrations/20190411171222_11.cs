using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProfileValue");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "ProfileValue",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Profile",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                table: "Profile",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "OrganizationUser",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Organization",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "AssociatedOrganizationUser",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Profile");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "ProfileValue",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedAt",
                table: "ProfileValue",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Profile",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "OrganizationUser",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Organization",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "AssociatedOrganizationUser",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
