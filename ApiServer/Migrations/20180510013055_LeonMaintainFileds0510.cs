using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonMaintainFileds0510 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Textures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "StaticMeshs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Solutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Solutions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Solutions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Skirtings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Skirtings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Skirtings",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductSpec",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "ProductSpec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "ProductSpec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "ProductSpec",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "OrganMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "OrganMember",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrganMember",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "OrganMember",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Materials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Layouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Layouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Layouts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Departments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "ClientAssets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "ClientAssets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "ClientAssets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetTags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AssetTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "AssetTags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetFolders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AssetFolders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "AssetFolders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Accounts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssetCategoryTrees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LValue = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    ObjId = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    RValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategoryTrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTrees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LValue = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NodeType = table.Column<string>(nullable: true),
                    ObjId = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    RValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTrees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetCategoryTrees");

            migrationBuilder.DropTable(
                name: "PermissionTrees");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ProductSpec",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
