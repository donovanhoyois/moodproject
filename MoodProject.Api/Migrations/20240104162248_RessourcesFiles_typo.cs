using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class RessourcesFiles_typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RessourceFiles_Ressources_RessourceId",
                table: "RessourceFiles");

            migrationBuilder.DropIndex(
                name: "IX_RessourceFiles_RessourceId",
                table: "RessourceFiles");

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "RessourceFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RessourceFiles_ResourceId",
                table: "RessourceFiles",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RessourceFiles_Ressources_ResourceId",
                table: "RessourceFiles",
                column: "ResourceId",
                principalTable: "Ressources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RessourceFiles_Ressources_ResourceId",
                table: "RessourceFiles");

            migrationBuilder.DropIndex(
                name: "IX_RessourceFiles_ResourceId",
                table: "RessourceFiles");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "RessourceFiles");

            migrationBuilder.CreateIndex(
                name: "IX_RessourceFiles_RessourceId",
                table: "RessourceFiles",
                column: "RessourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RessourceFiles_Ressources_RessourceId",
                table: "RessourceFiles",
                column: "RessourceId",
                principalTable: "Ressources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
