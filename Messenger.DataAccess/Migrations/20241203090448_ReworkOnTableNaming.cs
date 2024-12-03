using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReworkOnTableNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_ChatsId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Users_ParticipantsId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageUser_Messages_MessageId",
                table: "MessageUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageUser_Users_ReadById",
                table: "MessageUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MessageId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageUser",
                table: "MessageUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "MessageUser",
                newName: "MessagesReadBy");

            migrationBuilder.RenameTable(
                name: "ChatUser",
                newName: "UsersToChats");

            migrationBuilder.RenameIndex(
                name: "IX_MessageUser_ReadById",
                table: "MessagesReadBy",
                newName: "IX_MessagesReadBy_ReadById");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUser_ParticipantsId",
                table: "UsersToChats",
                newName: "IX_UsersToChats_ParticipantsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesReadBy",
                table: "MessagesReadBy",
                columns: new[] { "MessageId", "ReadById" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersToChats",
                table: "UsersToChats",
                columns: new[] { "ChatsId", "ParticipantsId" });

            migrationBuilder.CreateTable(
                name: "NewUnreadMessagesNotifiedBy",
                columns: table => new
                {
                    Message1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationReceivedById = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewUnreadMessagesNotifiedBy", x => new { x.Message1Id, x.NotificationReceivedById });
                    table.ForeignKey(
                        name: "FK_NewUnreadMessagesNotifiedBy_Messages_Message1Id",
                        column: x => x.Message1Id,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewUnreadMessagesNotifiedBy_Users_NotificationReceivedById",
                        column: x => x.NotificationReceivedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewUnreadMessagesNotifiedBy_NotificationReceivedById",
                table: "NewUnreadMessagesNotifiedBy",
                column: "NotificationReceivedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesReadBy_Messages_MessageId",
                table: "MessagesReadBy",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesReadBy_Users_ReadById",
                table: "MessagesReadBy",
                column: "ReadById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersToChats_Chats_ChatsId",
                table: "UsersToChats",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersToChats_Users_ParticipantsId",
                table: "UsersToChats",
                column: "ParticipantsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesReadBy_Messages_MessageId",
                table: "MessagesReadBy");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesReadBy_Users_ReadById",
                table: "MessagesReadBy");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersToChats_Chats_ChatsId",
                table: "UsersToChats");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersToChats_Users_ParticipantsId",
                table: "UsersToChats");

            migrationBuilder.DropTable(
                name: "NewUnreadMessagesNotifiedBy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersToChats",
                table: "UsersToChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesReadBy",
                table: "MessagesReadBy");

            migrationBuilder.RenameTable(
                name: "UsersToChats",
                newName: "ChatUser");

            migrationBuilder.RenameTable(
                name: "MessagesReadBy",
                newName: "MessageUser");

            migrationBuilder.RenameIndex(
                name: "IX_UsersToChats_ParticipantsId",
                table: "ChatUser",
                newName: "IX_ChatUser_ParticipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesReadBy_ReadById",
                table: "MessageUser",
                newName: "IX_MessageUser_ReadById");

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "ChatsId", "ParticipantsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageUser",
                table: "MessageUser",
                columns: new[] { "MessageId", "ReadById" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_MessageId",
                table: "Users",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_ChatsId",
                table: "ChatUser",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Users_ParticipantsId",
                table: "ChatUser",
                column: "ParticipantsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageUser_Messages_MessageId",
                table: "MessageUser",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageUser_Users_ReadById",
                table: "MessageUser",
                column: "ReadById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
