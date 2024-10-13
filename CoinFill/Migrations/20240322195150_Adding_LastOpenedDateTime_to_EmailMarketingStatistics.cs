using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CoinFill.Migrations
{
    public partial class Adding_LastOpenedDateTime_to_EmailMarketingStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastOpenedDateTime",
                table: "EmailMarketingStatistics",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
