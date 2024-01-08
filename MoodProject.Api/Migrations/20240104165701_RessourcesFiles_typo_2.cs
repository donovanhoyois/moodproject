using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class RessourcesFiles_typo_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RessourceFiles_Ressources_ResourceId",
                table: "RessourceFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ressources",
                table: "Ressources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RessourceFiles",
                table: "RessourceFiles");

            migrationBuilder.RenameTable(
                name: "Ressources",
                newName: "Resources");

            migrationBuilder.RenameTable(
                name: "RessourceFiles",
                newName: "ResourceFiles");

            migrationBuilder.RenameIndex(
                name: "IX_RessourceFiles_ResourceId",
                table: "ResourceFiles",
                newName: "IX_ResourceFiles_ResourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resources",
                table: "Resources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceFiles",
                table: "ResourceFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceFiles_Resources_ResourceId",
                table: "ResourceFiles",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceFiles_Resources_ResourceId",
                table: "ResourceFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resources",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceFiles",
                table: "ResourceFiles");

            migrationBuilder.RenameTable(
                name: "Resources",
                newName: "Ressources");

            migrationBuilder.RenameTable(
                name: "ResourceFiles",
                newName: "RessourceFiles");

            migrationBuilder.RenameIndex(
                name: "IX_ResourceFiles_ResourceId",
                table: "RessourceFiles",
                newName: "IX_RessourceFiles_ResourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ressources",
                table: "Ressources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RessourceFiles",
                table: "RessourceFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RessourceFiles_Ressources_ResourceId",
                table: "RessourceFiles",
                column: "ResourceId",
                principalTable: "Ressources",
                principalColumn: "Id");
        }
    }
}
