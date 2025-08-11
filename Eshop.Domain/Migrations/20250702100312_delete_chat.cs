using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class delete_chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats",
                schema: "ChatManagement");

            migrationBuilder.DropTable(
                name: "ChatRooms",
                schema: " ChatManagement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: " ChatManagement");

            migrationBuilder.EnsureSchema(
                name: "ChatManagement");

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                schema: " ChatManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingUserId = table.Column<long>(type: "bigint", nullable: false),
                    SendingUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRooms_Users_ReceivingUserId",
                        column: x => x.ReceivingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatRooms_Users_SendingUserId",
                        column: x => x.SendingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                schema: "ChatManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalSchema: " ChatManagement",
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_ReceivingUserId",
                schema: " ChatManagement",
                table: "ChatRooms",
                column: "ReceivingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_SendingUserId",
                schema: " ChatManagement",
                table: "ChatRooms",
                column: "SendingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatRoomId",
                schema: "ChatManagement",
                table: "Chats",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                schema: "ChatManagement",
                table: "Chats",
                column: "UserId");
        }
    }
}
