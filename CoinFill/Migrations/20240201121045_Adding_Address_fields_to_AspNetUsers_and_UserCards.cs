using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFill.Migrations
{
    public partial class Adding_Address_fields_to_AspNetUsers_and_UserCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "AspNetUsers",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameAddress",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                maxLength: 128,
                nullable: true);

            /////////////////////////////////////

            migrationBuilder.AddColumn<string>(
                name: "FullNameAddress",
                table: "UserCards",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "UserCards",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "UserCards",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserCards",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "UserCards",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserCards",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
