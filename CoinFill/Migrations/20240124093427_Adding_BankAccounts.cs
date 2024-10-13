using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CoinFill.Migrations
{
    public partial class Adding_BankAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBankAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: true),
                    FirstName = table.Column<string>(maxLength: 128, nullable: true),
                    LastName = table.Column<string>(maxLength: 128, nullable: true),
                    Currency = table.Column<string>(maxLength: 128, nullable: true),
                    BankAccountNumber = table.Column<string>(maxLength: 128, nullable: true),
                    BicSwift = table.Column<string>(maxLength: 128, nullable: true),
                    RoutingNumber = table.Column<string>(maxLength: 128, nullable: true),
                    TransitNumber = table.Column<string>(maxLength: 128, nullable: true),
                    InstitutionNumber = table.Column<string>(maxLength: 128, nullable: true),
                    AddedDateTime = table.Column<DateTime>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
