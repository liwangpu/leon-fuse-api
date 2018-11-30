using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.Basic.Service.Migrations
{
    public partial class NavigationAddDes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Navigations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Navigations");
        }
    }
}
