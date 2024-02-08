using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cube.EntityFramework.Migrations
{
    public partial class RenameModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_ChatAdminId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_ChatModelId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ChatModelId",
                table: "Users",
                newName: "ChatEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ChatModelId",
                table: "Users",
                newName: "IX_Users_ChatEntityId");

            migrationBuilder.RenameColumn(
                name: "ChatAdminId",
                table: "Chats",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ChatAdminId",
                table: "Chats",
                newName: "IX_Chats_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_AdminId",
                table: "Chats",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_ChatEntityId",
                table: "Users",
                column: "ChatEntityId",
                principalTable: "Chats",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_AdminId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_ChatEntityId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ChatEntityId",
                table: "Users",
                newName: "ChatModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ChatEntityId",
                table: "Users",
                newName: "IX_Users_ChatModelId");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Chats",
                newName: "ChatAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_AdminId",
                table: "Chats",
                newName: "IX_Chats_ChatAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_ChatAdminId",
                table: "Chats",
                column: "ChatAdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_ChatModelId",
                table: "Users",
                column: "ChatModelId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
