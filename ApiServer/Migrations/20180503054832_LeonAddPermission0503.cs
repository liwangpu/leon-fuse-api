using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddPermission0503 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Solutions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Solutions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Skirtings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Skirtings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "ProductSpec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "ProductSpec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "OrganMember",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "OrganMember",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Layouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Layouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "ClientAssets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "ClientAssets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AssetTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "AssetTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AssetFolders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "AssetFolders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Accounts",
                nullable: true);

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
                name: "PermissionTrees");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Accounts");
        }
    }
}
