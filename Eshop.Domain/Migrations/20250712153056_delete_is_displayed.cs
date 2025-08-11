using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class delete_is_displayed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "TicketsMessages");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "AboutUs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "TicketsMessages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "Ticket",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "SiteSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "Roles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "ContactUs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "AboutUs",
                type: "bit",
                nullable: true);
        }
    }
}
