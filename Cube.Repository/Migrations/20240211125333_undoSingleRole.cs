using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cube.EntityFramework.Migrations
{
    public partial class undoSingleRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Accounts",
                newName: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "Accounts",
                newName: "Role");
        }
    }
}
