using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cube.Repository.Migrations
{
    public partial class UpdateNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Notifications");
        }
    }
}
