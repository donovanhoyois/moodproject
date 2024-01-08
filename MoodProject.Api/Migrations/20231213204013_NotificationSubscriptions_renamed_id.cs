using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class NotificationSubscriptions_renamed_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationSubscriptionId",
                table: "NotificationSubscriptions",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NotificationSubscriptions",
                newName: "NotificationSubscriptionId");
        }
    }
}
