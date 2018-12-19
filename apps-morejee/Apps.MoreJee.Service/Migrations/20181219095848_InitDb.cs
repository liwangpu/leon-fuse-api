using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.MoreJee.Service.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetCategories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    IsRoot = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    DisplayIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetCategoryTrees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LValue = table.Column<int>(nullable: false),
                    RValue = table.Column<int>(nullable: false),
                    ParentId = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    ObjId = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategoryTrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
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
                    FileAssetId = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    UnCookedAssetId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    FileAssetId = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Parameters = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    UnCookedAssetId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticMeshs",
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
                    Icon = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileAssetId = table.Column<string>(nullable: true),
                    Dependencies = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    SrcFileAssetId = table.Column<string>(nullable: true),
                    Materials = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    UnCookedAssetId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticMeshs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetCategories");

            migrationBuilder.DropTable(
                name: "AssetCategoryTrees");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "StaticMeshs");
        }
    }
}
