using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddTypeToMedia0605 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "Medias",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Medias",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolutionId",
                table: "Medias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "SolutionId",
                table: "Medias");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Medias",
                newName: "Direction");
        }
    }
}
