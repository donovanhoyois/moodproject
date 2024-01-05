using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class RessourcesFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RessourceFile_Ressources_RessourceId",
                table: "RessourceFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RessourceFile",
                table: "RessourceFile");

            migrationBuilder.RenameTable(
                name: "RessourceFile",
                newName: "RessourceFiles");

            migrationBuilder.RenameIndex(
                name: "IX_RessourceFile_RessourceId",
                table: "RessourceFiles",
                newName: "IX_RessourceFiles_RessourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RessourceFiles",
                table: "RessourceFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RessourceFiles_Ressources_RessourceId",
                table: "RessourceFiles",
                column: "RessourceId",
                principalTable: "Ressources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RessourceFiles_Ressources_RessourceId",
                table: "RessourceFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RessourceFiles",
                table: "RessourceFiles");

            migrationBuilder.RenameTable(
                name: "RessourceFiles",
                newName: "RessourceFile");

            migrationBuilder.RenameIndex(
                name: "IX_RessourceFiles_RessourceId",
                table: "RessourceFile",
                newName: "IX_RessourceFile_RessourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RessourceFile",
                table: "RessourceFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RessourceFile_Ressources_RessourceId",
                table: "RessourceFile",
                column: "RessourceId",
                principalTable: "Ressources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
