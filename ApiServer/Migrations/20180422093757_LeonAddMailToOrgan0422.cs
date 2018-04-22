using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddMailToOrgan0422 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Organizations");
        }
    }
}
