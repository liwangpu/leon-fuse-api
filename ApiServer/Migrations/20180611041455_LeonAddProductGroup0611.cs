﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddProductGroup0611 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Items = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    Orientation = table.Column<int>(nullable: false),
                    PivotLocation = table.Column<string>(nullable: true),
                    PivotType = table.Column<int>(nullable: false),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGroups");
        }
    }
}
