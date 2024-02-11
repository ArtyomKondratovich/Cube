using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cube.EntityFramework.Migrations
{
    public partial class undoChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
