using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class RoomChatComment_fk_ChatRoomPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_PostId",
                table: "ChatRoomComments");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomComments_PostId",
                table: "ChatRoomComments");

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomPostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomComments_ChatRoomPostId",
                table: "ChatRoomComments",
                column: "ChatRoomPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments",
                column: "ChatRoomPostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomComments_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.DropColumn(
                name: "ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomComments_PostId",
                table: "ChatRoomComments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_PostId",
                table: "ChatRoomComments",
                column: "PostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
