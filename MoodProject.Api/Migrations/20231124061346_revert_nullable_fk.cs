using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class revert_nullable_fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_PostId",
                table: "ChatRoomComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_RoomId",
                table: "ChatRoomPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Symptoms_SymptomId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomPosts_RoomId",
                table: "ChatRoomPosts");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomComments_PostId",
                table: "ChatRoomComments");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "ChatRoomPosts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "ChatRoomComments");

            migrationBuilder.AlterColumn<int>(
                name: "SymptomId",
                table: "ChatRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomId",
                table: "ChatRoomPosts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomPostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomPosts_ChatRoomId",
                table: "ChatRoomPosts",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomComments_ChatRoomPostId",
                table: "ChatRoomComments",
                column: "ChatRoomPostId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Symptoms_SymptomId",
                table: "ChatRooms",
                column: "SymptomId",
                principalTable: "Symptoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_ChatRoomId",
                table: "ChatRoomPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Symptoms_SymptomId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomPosts_ChatRoomId",
                table: "ChatRoomPosts");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoomComments_ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.DropColumn(
                name: "ChatRoomId",
                table: "ChatRoomPosts");

            migrationBuilder.DropColumn(
                name: "ChatRoomPostId",
                table: "ChatRoomComments");

            migrationBuilder.AlterColumn<int>(
                name: "SymptomId",
                table: "ChatRooms",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "ChatRoomPosts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomPosts_RoomId",
                table: "ChatRoomPosts",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomComments_PostId",
                table: "ChatRoomComments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomComments_ChatRoomPosts_PostId",
                table: "ChatRoomComments",
                column: "PostId",
                principalTable: "ChatRoomPosts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomPosts_ChatRooms_RoomId",
                table: "ChatRoomPosts",
                column: "RoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Symptoms_SymptomId",
                table: "ChatRooms",
                column: "SymptomId",
                principalTable: "Symptoms",
                principalColumn: "Id");
        }
    }
}
