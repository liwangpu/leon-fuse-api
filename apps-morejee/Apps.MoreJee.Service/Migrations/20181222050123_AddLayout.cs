using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.MoreJee.Service.Migrations
{
    public partial class AddLayout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Maps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Maps");
        }
    }
}
