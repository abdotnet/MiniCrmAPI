using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCrm.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateuserschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                schema: "user",
                table: "Users",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemRole",
                schema: "user",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                schema: "user",
                table: "Permissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "user",
                table: "UserRoles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                schema: "user",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                schema: "user",
                table: "Permissions",
                column: "RoleId",
                principalSchema: "user",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                schema: "user",
                table: "UserRoles",
                column: "UserId",
                principalSchema: "user",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                schema: "user",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                schema: "user",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                schema: "user",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_RoleId",
                schema: "user",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsSystemRole",
                schema: "user",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "user",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                schema: "user",
                table: "Users",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
