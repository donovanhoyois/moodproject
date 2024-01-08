using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class MedicationDayUsage_utcOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtcOffset",
                table: "MedicationDayUsages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtcOffset",
                table: "MedicationDayUsages");
        }
    }
}
