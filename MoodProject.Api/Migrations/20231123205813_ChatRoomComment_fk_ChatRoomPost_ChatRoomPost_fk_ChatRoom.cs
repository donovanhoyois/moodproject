using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChatRoomComment_fk_ChatRoomPost_ChatRoomPost_fk_ChatRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_ChatRoomId",
                table: "ChatRoomPosts");

            migrationBuilder.RenameColumn(
                name: "ChatRoomId",
                table: "ChatRoomPosts",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomPosts_ChatRoomId",
                table: "ChatRoomPosts",
                newName: "IX_ChatRoomPosts_RoomId");

            migrationBuilder.RenameColumn(
                name: "ChatRoomPostId",
                table: "ChatRoomComments",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomComments_ChatRoomPostId",
                table: "ChatRoomComments",
                newName: "IX_ChatRoomComments_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_PostId",
                table: "ChatRoomComments",
                column: "PostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_RoomId",
                table: "ChatRoomPosts",
                column: "RoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_PostId",
                table: "ChatRoomComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_RoomId",
                table: "ChatRoomPosts");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "ChatRoomPosts",
                newName: "ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomPosts_RoomId",
                table: "ChatRoomPosts",
                newName: "IX_ChatRoomPosts_ChatRoomId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "ChatRoomComments",
                newName: "ChatRoomPostId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomComments_PostId",
                table: "ChatRoomComments",
                newName: "IX_ChatRoomComments_ChatRoomPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments",
                column: "ChatRoomPostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_ChatRoomId",
                table: "ChatRoomPosts",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
