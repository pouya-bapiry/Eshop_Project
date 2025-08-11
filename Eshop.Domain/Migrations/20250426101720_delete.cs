using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDelete",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDelete",
                table: "Roles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IsDelete",
                table: "Users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "IsDelete",
                table: "Roles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
