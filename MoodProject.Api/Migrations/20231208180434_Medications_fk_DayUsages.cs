using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class Medications_fk_DayUsages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationDayUsage_Medications_MedicationId",
                table: "MedicationDayUsage");

            migrationBuilder.DropTable(
                name: "MedicationSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicationDayUsage",
                table: "MedicationDayUsage");

            migrationBuilder.RenameTable(
                name: "MedicationDayUsage",
                newName: "MedicationDayUsages");

            migrationBuilder.RenameIndex(
                name: "IX_MedicationDayUsage_MedicationId",
                table: "MedicationDayUsages",
                newName: "IX_MedicationDayUsages_MedicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicationDayUsages",
                table: "MedicationDayUsages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationDayUsages_Medications_MedicationId",
                table: "MedicationDayUsages",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationDayUsages_Medications_MedicationId",
                table: "MedicationDayUsages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicationDayUsages",
                table: "MedicationDayUsages");

            migrationBuilder.RenameTable(
                name: "MedicationDayUsages",
                newName: "MedicationDayUsage");

            migrationBuilder.RenameIndex(
                name: "IX_MedicationDayUsages_MedicationId",
                table: "MedicationDayUsage",
                newName: "IX_MedicationDayUsage_MedicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicationDayUsage",
                table: "MedicationDayUsage",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MedicationSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MedicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationSchedules_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSchedules_MedicationId",
                table: "MedicationSchedules",
                column: "MedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationDayUsage_Medications_MedicationId",
                table: "MedicationDayUsage",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
