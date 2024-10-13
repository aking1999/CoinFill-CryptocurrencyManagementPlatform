using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class Adding_Tables_Validators_and_ValidatorsCryptocurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Validators",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Photo = table.Column<string>(maxLength: 1024, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    AvailableCryptocurrenciesToStake = table.Column<int>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidatorsCryptocurrencies",
                columns: table => new
                {
                    ValidatorId = table.Column<string>(maxLength: 450, nullable: false),
                    CryptocurrencyId = table.Column<string>(maxLength: 450, nullable: false),
                    Apy = table.Column<double>(maxLength: 512, nullable: false, defaultValue: 0),
                    TotalStaked = table.Column<double>(maxLength: 512, nullable: false, defaultValue: 0),
                    UnlockTimeHours = table.Column<double>(maxLength: 128, nullable: false, defaultValue: 1),
                    MinimumDepositAmount = table.Column<double>(maxLength: 128, nullable: false, defaultValue: 1),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidatorsCryptocurrencies", x => new { x.ValidatorId, x.CryptocurrencyId });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ValidatorsCryptocurrencies_Validators_Id",
                table: "ValidatorsCryptocurrencies",
                column: "ValidatorId",
                principalTable: "Validators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidatorsCryptocurrencies_Cryptocurrencies_Id",
                table: "ValidatorsCryptocurrencies",
                column: "CryptocurrencyId",
                principalTable: "Cryptocurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBankAccounts");
        }
    }
}
