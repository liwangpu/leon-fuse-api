using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.Basic.Service.Migrations
{
    public partial class UseroleAddIsInner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInner",
                table: "UserRoles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInner",
                table: "UserRoles");
        }
    }
}
