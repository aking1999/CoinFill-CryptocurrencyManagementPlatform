using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CoinFill.Data.Migrations
{
    public partial class Adding_ErrorLogs_Newsletter_UserActivityLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    UserIdOrAnonymous = table.Column<string>(maxLength: 450, nullable: true),
                    AreaOrProject = table.Column<string>(maxLength: 64, nullable: true),
                    ControllerOrClass = table.Column<string>(maxLength: 64, nullable: true),
                    ActionOrMethod = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    Source = table.Column<string>(maxLength: 512, nullable: true),
                    StackTraceFrameMethodName = table.Column<string>(maxLength: 512, nullable: true),
                    StackTraceExecutingAssemblyName = table.Column<string>(maxLength: 512, nullable: true),
                    TargetSiteName = table.Column<string>(maxLength: 512, nullable: true),
                    TargetSiteReflectedTypeFullName = table.Column<string>(maxLength: 512, nullable: true),
                    StackTrace = table.Column<string>(maxLength: 4096, nullable: true),
                    ErrorDateTime = table.Column<DateTime>(maxLength: 128, nullable: true),
                    Fixed = table.Column<bool>(maxLength: 128, nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsletterSubscribers",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: false),
                    NotifiedCount = table.Column<int>(maxLength: 32, nullable: false, defaultValue: 0),
                    IpAddress = table.Column<string>(maxLength: 256, nullable: true),
                    LastNotifiedDateTime = table.Column<DateTime>(maxLength: 128, nullable: true),
                    SubscribeDateTime = table.Column<DateTime>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterSubscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserActivityLogs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    UserIdOrAnonymous = table.Column<string>(maxLength: 450, nullable: false),
                    Area = table.Column<string>(maxLength: 64, nullable: true),
                    Controller = table.Column<string>(maxLength: 64, nullable: true),
                    Action = table.Column<string>(maxLength: 64, nullable: true),
                    QueryDataJson = table.Column<string>(maxLength: 2048, nullable: true),
                    MethodType = table.Column<string>(maxLength: 64, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 256, nullable: true),
                    UserAgent = table.Column<string>(maxLength: 1024, nullable: true),
                    ActivityDateTime = table.Column<DateTime>(maxLength: 128, nullable: true),
                    IsCrawler = table.Column<bool>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
