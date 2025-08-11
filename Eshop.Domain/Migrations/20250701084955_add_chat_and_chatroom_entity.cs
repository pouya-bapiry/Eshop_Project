using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class add_chat_and_chatroom_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketMessage_Ticket_TicketId",
                table: "TicketMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketMessage_Users_SenderId",
                table: "TicketMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketMessage",
                table: "TicketMessage");

            migrationBuilder.EnsureSchema(
                name: " ChatManagement");

            migrationBuilder.EnsureSchema(
                name: "ChatManagement");

            migrationBuilder.RenameTable(
                name: "TicketMessage",
                newName: "TicketsMessages");

            migrationBuilder.RenameIndex(
                name: "IX_TicketMessage_TicketId",
                table: "TicketsMessages",
                newName: "IX_TicketsMessages_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketMessage_SenderId",
                table: "TicketsMessages",
                newName: "IX_TicketsMessages_SenderId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "Users",
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

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayed",
                table: "TicketsMessages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketsMessages",
                table: "TicketsMessages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                schema: " ChatManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendingUserId = table.Column<long>(type: "bigint", nullable: false),
                    ReceivingUserId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Text = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsMessages_Ticket_TicketId",
                table: "TicketsMessages",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsMessages_Users_SenderId",
                table: "TicketsMessages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsMessages_Ticket_TicketId",
                table: "TicketsMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsMessages_Users_SenderId",
                table: "TicketsMessages");

            migrationBuilder.DropTable(
                name: "Chats",
                schema: "ChatManagement");

            migrationBuilder.DropTable(
                name: "ChatRooms",
                schema: " ChatManagement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketsMessages",
                table: "TicketsMessages");

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "Users");

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

            migrationBuilder.DropColumn(
                name: "IsDisplayed",
                table: "TicketsMessages");

            migrationBuilder.RenameTable(
                name: "TicketsMessages",
                newName: "TicketMessage");

            migrationBuilder.RenameIndex(
                name: "IX_TicketsMessages_TicketId",
                table: "TicketMessage",
                newName: "IX_TicketMessage_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketsMessages_SenderId",
                table: "TicketMessage",
                newName: "IX_TicketMessage_SenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketMessage",
                table: "TicketMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMessage_Ticket_TicketId",
                table: "TicketMessage",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMessage_Users_SenderId",
                table: "TicketMessage",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
