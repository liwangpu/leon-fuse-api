using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonAddMediaShareResource0605 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "StartShareTimeStamp",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "StopShareTimeStamp",
                table: "Medias");

            migrationBuilder.CreateTable(
                name: "MediaShareResources",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MediaId = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ResourceType = table.Column<int>(nullable: false),
                    StartShareTimeStamp = table.Column<long>(nullable: false),
                    StopShareTimeStamp = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaShareResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaShareResources_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaShareResources_MediaId",
                table: "MediaShareResources",
                column: "MediaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaShareResources");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Medias",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StartShareTimeStamp",
                table: "Medias",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StopShareTimeStamp",
                table: "Medias",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
