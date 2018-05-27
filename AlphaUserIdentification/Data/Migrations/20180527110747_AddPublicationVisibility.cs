using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AlphaUserIdentification.Data.Migrations
{
    public partial class AddPublicationVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Publications",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "Publications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Publications");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Publications",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
