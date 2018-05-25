using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddPackageNameToStaticMesh0525 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Textures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "StaticMeshs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Maps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperty1",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Textures");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "StaticMeshs");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ExtraProperty1",
                table: "Files");
        }
    }
}
