using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class RoomChatComment_fk_ChatRoomPostId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "ChatRoomComments");

            migrationBuilder.AlterColumn<int>(
                name: "ChatRoomPostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments",
                column: "ChatRoomPostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.AlterColumn<int>(
                name: "ChatRoomPostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments",
                column: "ChatRoomPostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id");
        }
    }
}
