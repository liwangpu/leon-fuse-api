using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddCategoryToBasicEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "ProductSpec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Packages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "OrganMember",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "MediaShareResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Medias",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Collections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "AssetTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "AreaTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OrganMember");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MediaShareResources");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AssetTags");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AreaTypes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Accounts");
        }
    }
}
