using Microsoft.EntityFrameworkCore.Migrations;

namespace Apps.Basic.Service.Migrations
{
    public partial class AccountAddIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Accounts");
        }
    }
}
