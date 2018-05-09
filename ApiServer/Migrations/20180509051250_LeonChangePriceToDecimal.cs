using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiServer.Migrations
{
    public partial class LeonChangePriceToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductSpec",
                nullable: false,
                oldClrType: typeof(double));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ProductSpec",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
