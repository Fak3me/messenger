using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReworkOnEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Users_MessageId",
                table: "Users",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MessageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
