using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CoinFill.Migrations
{
    public partial class Adding_Cryptocurrency_Tables_and_Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Notifications",
               columns: table => new
               {
                   Id = table.Column<string>(maxLength: 450, nullable: false),
                   ReceiverUserId = table.Column<string>(maxLength: 450, nullable: false),
                   Title = table.Column<string>(maxLength: 1024, nullable: false),
                   Body = table.Column<string>(maxLength: 384, nullable: true),
                   Severity = table.Column<string>(maxLength: 64, nullable: false),
                   Icon = table.Column<string>(maxLength: 32, nullable: true),
                   Read = table.Column<bool>(maxLength: 128, nullable: false, defaultValue: false),
                   Important = table.Column<bool>(maxLength: 128, nullable: false, defaultValue: false),
                   SendingDateTime = table.Column<DateTime>(maxLength: 128, nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Notifications", x => x.Id);

                   table.ForeignKey(
                       name: "FK_NotificationsReceiverUserId_AspNetUsersId",
                       column: x => x.ReceiverUserId,
                       principalTable: "AspNetUsers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateTable(
                name: "Cryptocurrencies",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Color = table.Column<string>(maxLength: 64, nullable: false),
                    Icon = table.Column<string>(maxLength: 1024, nullable: false),
                    OrderNumber = table.Column<int>(maxLength: 64, nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptocurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptocurrencyNetworks",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    CryptocurrencyId = table.Column<string>(maxLength: 450, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Address = table.Column<string>(maxLength: 512, nullable: false),
                    QrImage = table.Column<string>(maxLength: 1024, nullable: false),
                    OrderNumber = table.Column<int>(maxLength: 64, nullable: false),
                    UsedCount = table.Column<int>(maxLength: 64, nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptocurrencyNetworks", x => x.Id);

                    table.ForeignKey(
                        name: "FK_CryptocurrencyNetworksId_AspNetUsersId",
                        column: x => x.CryptocurrencyId,
                        principalTable: "Cryptocurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedCryptocurrencyNetworks",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 450, nullable: false),
                    CryptocurrencyId = table.Column<string>(maxLength: 450, nullable: false),
                    CryptocurrencyNetworkId = table.Column<string>(maxLength: 450, nullable: false),
                    GenerationDateTime = table.Column<DateTime>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedCryptocurrencyNetworks", x => new { x.UserId, x.CryptocurrencyId, x.CryptocurrencyNetworkId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
