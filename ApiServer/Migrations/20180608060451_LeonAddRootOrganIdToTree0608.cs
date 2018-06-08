using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddRootOrganIdToTree0608 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Accounts_OwnerId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "RootOrganizationId",
                table: "PermissionTrees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RootOrganizationId",
                table: "AssetCategoryTrees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OrganizationId",
                table: "Accounts",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Organizations_OrganizationId",
                table: "Accounts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Organizations_OrganizationId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_OrganizationId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "RootOrganizationId",
                table: "PermissionTrees");

            migrationBuilder.DropColumn(
                name: "RootOrganizationId",
                table: "AssetCategoryTrees");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Accounts_OwnerId",
                table: "Organizations",
                column: "OwnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
