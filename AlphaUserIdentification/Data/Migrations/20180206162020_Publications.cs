using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AlphaUserIdentification.Data.Migrations
{
    public partial class Publications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Publications",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Publications");
        }
    }
}
