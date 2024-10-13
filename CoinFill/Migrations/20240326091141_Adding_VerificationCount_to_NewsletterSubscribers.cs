using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class Adding_VerificationCount_to_NewsletterSubscribers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "VerificationCount",
                table: "NewsletterSubscribers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
