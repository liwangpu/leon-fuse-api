using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddProductReplaceGroup0711 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductReplaceGroups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    DefaultItemId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GroupItemIds = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReplaceGroups", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReplaceGroups");
        }
    }
}
