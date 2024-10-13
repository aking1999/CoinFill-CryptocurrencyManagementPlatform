using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CoinFill.Migrations
{
    public partial class Adding_Payments_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: true),
                    Reason = table.Column<string>(maxLength: 512, nullable: true),
                    ReasonId = table.Column<string>(maxLength: 450, nullable: true),
                    AmountShouldBe = table.Column<decimal>(maxLength: 128, nullable: false, defaultValue: 0),
                    PaymentMethodCryptocurrencyId = table.Column<string>(maxLength: 450, nullable: true),
                    PaymentMethodCryptocurrencyNetworkId = table.Column<string>(maxLength: 450, nullable: true),
                    ActivationStatus = table.Column<int>(maxLength: 128, nullable: false, defaultValue: 0),
                    CreatedDateTime = table.Column<DateTime>(maxLength: 128, nullable: true),
                    VerifiedDateTime = table.Column<DateTime>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
