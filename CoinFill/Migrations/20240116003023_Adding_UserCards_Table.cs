using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class Adding_UserCards_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCards",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: true),
                    Type = table.Column<string>(maxLength: 128, nullable: true),
                    Brand = table.Column<string>(maxLength: 128, nullable: true),
                    Number = table.Column<string>(maxLength: 128, nullable: true),
                    ExpirationDate = table.Column<string>(maxLength: 128, nullable: true),
                    CVV = table.Column<string>(maxLength: 128, nullable: true),
                    Balance = table.Column<decimal>(maxLength: 128, nullable: false, defaultValue: 0),
                    BackgroundImage = table.Column<decimal>(maxLength: 1024, nullable: true),
                    PaymentMethodCryptocurrencyId = table.Column<string>(maxLength: 450, nullable: true),
                    PaymentMethodCryptocurrencyNetworkId = table.Column<string>(maxLength: 450, nullable: true),
                    PaymentWalletAddress = table.Column<string>(maxLength: 4096, nullable: true),
                    ActivationStatus = table.Column<int>(maxLength: 128, nullable: false, defaultValue: 0),
                    RequestedDateTime = table.Column<DateTime>(maxLength: 128, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(maxLength: 128, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCards", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
