using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class add_relation_between_user_and_banner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SiteBanners",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteBanners_UserId",
                table: "SiteBanners",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteBanners_Users_UserId",
                table: "SiteBanners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteBanners_Users_UserId",
                table: "SiteBanners");

            migrationBuilder.DropIndex(
                name: "IX_SiteBanners_UserId",
                table: "SiteBanners");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SiteBanners");
        }
    }
}
