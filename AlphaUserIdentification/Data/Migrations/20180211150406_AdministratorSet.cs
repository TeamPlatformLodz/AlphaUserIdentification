using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AlphaUserIdentification.Data.Migrations
{
    public partial class AdministratorSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrator_AspNetUsers_ApplicationUserId",
                table: "Administrator");

            migrationBuilder.DropForeignKey(
                name: "FK_Administrator_Teams_TeamId",
                table: "Administrator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator");

            migrationBuilder.RenameTable(
                name: "Administrator",
                newName: "Administrators");

            migrationBuilder.RenameIndex(
                name: "IX_Administrator_TeamId",
                table: "Administrators",
                newName: "IX_Administrators_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrators",
                table: "Administrators",
                columns: new[] { "ApplicationUserId", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_AspNetUsers_ApplicationUserId",
                table: "Administrators",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Teams_TeamId",
                table: "Administrators",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_AspNetUsers_ApplicationUserId",
                table: "Administrators");

            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Teams_TeamId",
                table: "Administrators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrators",
                table: "Administrators");

            migrationBuilder.RenameTable(
                name: "Administrators",
                newName: "Administrator");

            migrationBuilder.RenameIndex(
                name: "IX_Administrators_TeamId",
                table: "Administrator",
                newName: "IX_Administrator_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator",
                columns: new[] { "ApplicationUserId", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Administrator_AspNetUsers_ApplicationUserId",
                table: "Administrator",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Administrator_Teams_TeamId",
                table: "Administrator",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
