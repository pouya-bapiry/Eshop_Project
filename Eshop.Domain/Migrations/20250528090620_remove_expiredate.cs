using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class remove_expiredate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileCodeExpireDate",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MobileCodeExpireDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }
    }
}
