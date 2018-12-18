using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.MoreJee.Service.Migrations
{
    public partial class MaterialAddIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Materials",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Materials");
        }
    }
}
