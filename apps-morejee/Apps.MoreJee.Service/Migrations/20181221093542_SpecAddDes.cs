using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.MoreJee.Service.Migrations
{
    public partial class SpecAddDes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductSpecs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductSpecs");
        }
    }
}
