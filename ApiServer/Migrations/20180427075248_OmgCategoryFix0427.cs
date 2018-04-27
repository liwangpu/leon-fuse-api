using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class OmgCategoryFix0427 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayIndex",
                table: "AssetCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_AccountId",
                table: "Materials",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Accounts_AccountId",
                table: "Materials",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Accounts_AccountId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_AccountId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DisplayIndex",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AssetCategories");
        }
    }
}
