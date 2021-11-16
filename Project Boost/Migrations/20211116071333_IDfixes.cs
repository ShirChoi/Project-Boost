using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectBoost.Migrations
{
    public partial class IDfixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_RoleID",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "ID");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "user" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Nickname", "OpenFinantialHistory", "Password", "Restricted", "RoleID" },
                values: new object[] { new Guid("dd68a389-4ad1-4e39-999c-d51edad34ef4"), "admin@mail.ru", false, "123456", false, 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("dd68a389-4ad1-4e39-999c-d51edad34ef4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
