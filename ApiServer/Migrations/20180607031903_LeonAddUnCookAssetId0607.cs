using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddUnCookAssetId0607 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnCookedAssetId",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SrcFileAssetId",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnCookedAssetId",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnCookedAssetId",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnCookedAssetId",
                table: "Maps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnCookedAssetId",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "SrcFileAssetId",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "UnCookedAssetId",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "UnCookedAssetId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "UnCookedAssetId",
                table: "Maps");
        }
    }
}
