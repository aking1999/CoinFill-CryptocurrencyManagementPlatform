using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class Update_Emails_Rows_Lengths_NewsletterSubscribers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NewsletterSubscribers",
                maxLength: 9000,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "NewsletterSubscribers",
                maxLength: 9000,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
