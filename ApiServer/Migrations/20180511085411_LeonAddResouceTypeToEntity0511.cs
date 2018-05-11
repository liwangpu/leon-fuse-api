using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddResouceTypeToEntity0511 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Textures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "StaticMeshs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Solutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Skirtings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "ProductSpec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Packages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "OrganMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Materials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Maps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Layouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Departments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "ClientAssets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "AssetTags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "AssetFolders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "AssetCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "ClientAssets");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "AssetFolders");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Accounts");
        }
    }
}
