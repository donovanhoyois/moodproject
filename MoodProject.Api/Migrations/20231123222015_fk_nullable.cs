using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class fk_nullable : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "SymptomId",
                table: "ChatRooms",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "ChatRoomPosts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "SymptomId",
                table: "ChatRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "ChatRoomPosts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "ChatRoomComments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Symptoms_SymptomId",
                table: "ChatRooms",
                column: "SymptomId",
                principalTable: "Symptoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
