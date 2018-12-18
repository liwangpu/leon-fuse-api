using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.MoreJee.Service.Migrations
{
    public partial class CategoryAddDes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssetCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "AssetCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssetCategories");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AssetCategories");
        }
    }
}
