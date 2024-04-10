using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cube.Repository.Migrations
{
    public partial class RenameColumnInFriendship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_FriendId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friendships",
                newName: "SecondUserId");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "Friendships",
                newName: "FirstUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                newName: "IX_Friendships_SecondUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendships",
                newName: "IX_Friendships_FirstUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_FirstUserId",
                table: "Friendships",
                column: "FirstUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_SecondUserId",
                table: "Friendships",
                column: "SecondUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_FirstUserId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_SecondUserId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "SecondUserId",
                table: "Friendships",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FirstUserId",
                table: "Friendships",
                newName: "FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_SecondUserId",
                table: "Friendships",
                newName: "IX_Friendships_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FirstUserId",
                table: "Friendships",
                newName: "IX_Friendships_FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_FriendId",
                table: "Friendships",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
