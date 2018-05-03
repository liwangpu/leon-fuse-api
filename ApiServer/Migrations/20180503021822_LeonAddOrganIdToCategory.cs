using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddOrganIdToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductSpec");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ProductSpec",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "AssetCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AssetCategories");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ProductSpec",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "ProductSpec",
                nullable: true);
        }
    }
}
