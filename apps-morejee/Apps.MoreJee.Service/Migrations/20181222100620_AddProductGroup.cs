using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.MoreJee.Service.Migrations
{
    public partial class AddProductGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PivotLocation = table.Column<string>(nullable: true),
                    PivotType = table.Column<int>(nullable: false),
                    Orientation = table.Column<int>(nullable: false),
                    Items = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductReplaceGroups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DefaultItemId = table.Column<string>(nullable: true),
                    GroupItemIds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReplaceGroups", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "ProductReplaceGroups");
        }
    }
}
