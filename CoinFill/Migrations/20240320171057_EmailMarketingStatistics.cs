using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class EmailMarketingStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailMarketingStatistics",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    CampaignName = table.Column<string>(maxLength: 1024, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    OpenCount = table.Column<int>(maxLength: 128, nullable: false, defaultValue: 0),
                    Unsubscribed = table.Column<bool>(maxLength: 64, nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMarketingStatistics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
