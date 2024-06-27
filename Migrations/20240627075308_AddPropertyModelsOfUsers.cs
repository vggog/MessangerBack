using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessangerBack.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyModelsOfUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatModelId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatModelId",
                table: "Users",
                column: "ChatModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_ChatModelId",
                table: "Users",
                column: "ChatModelId",
                principalTable: "Chats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_ChatModelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatModelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatModelId",
                table: "Users");
        }
    }
}
