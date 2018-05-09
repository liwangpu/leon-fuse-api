using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddPackage0509 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Textures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "StaticMeshs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Solutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Skirtings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "ProductSpec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "PermissionTrees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "OrganMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Materials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Layouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Departments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "ClientAssets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetTags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetFolders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetCategoryTrees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "AssetCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveFlag",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "PermissionTrees");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetCategoryTrees");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Accounts");
        }
    }
}
