using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cube.Repository.Migrations
{
    public partial class UpdateImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageBytes",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Images",
                newName: "Path");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Images",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Images",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Images",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Images",
                newName: "Name");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageBytes",
                table: "Images",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Name");
        }
    }
}
