using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class Changing_DataType_in_UserCards_BackgroundImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BackgroundImage",
                table: "UserCards",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
