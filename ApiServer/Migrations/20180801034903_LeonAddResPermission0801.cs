using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddResPermission0801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourcePermissions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OpDelete = table.Column<int>(nullable: false),
                    OpRetrieve = table.Column<int>(nullable: false),
                    OpUpdate = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResId = table.Column<string>(nullable: true),
                    ResType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePermissions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePermissions_OrganizationId_ResType",
                table: "ResourcePermissions",
                columns: new[] { "OrganizationId", "ResType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourcePermissions");
        }
    }
}
