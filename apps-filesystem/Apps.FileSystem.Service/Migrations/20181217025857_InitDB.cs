﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.FileSystem.Service.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileAssets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ActiveFlag = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Md5 = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    FileExt = table.Column<string>(nullable: true),
                    LocalPath = table.Column<string>(nullable: true),
                    FileState = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAssets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAssets");
        }
    }
}