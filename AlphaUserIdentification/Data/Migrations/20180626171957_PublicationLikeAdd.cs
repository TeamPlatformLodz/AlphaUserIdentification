using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AlphaUserIdentification.Data.Migrations
{
    public partial class PublicationLikeAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicationLikes",
                columns: table => new
                {
                    PublicationId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationLikes", x => new { x.PublicationId, x.ApplicationUserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicationLikes");
        }
    }
}
