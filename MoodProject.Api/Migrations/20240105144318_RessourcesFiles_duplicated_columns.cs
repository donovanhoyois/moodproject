using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class RessourcesFiles_duplicated_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceFiles_Resources_ResourceId",
                table: "ResourceFiles");

            migrationBuilder.DropColumn(
                name: "RessourceId",
                table: "ResourceFiles");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceId",
                table: "ResourceFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceFiles_Resources_ResourceId",
                table: "ResourceFiles",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceFiles_Resources_ResourceId",
                table: "ResourceFiles");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceId",
                table: "ResourceFiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "RessourceId",
                table: "ResourceFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceFiles_Resources_ResourceId",
                table: "ResourceFiles",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }
    }
}
