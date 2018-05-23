using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddOrganIdToBasicEntity0523 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Skirtings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "ProductSpec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Packages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "AssetTags",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Skirtings");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "ProductSpec");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AssetTags");
        }
    }
}
