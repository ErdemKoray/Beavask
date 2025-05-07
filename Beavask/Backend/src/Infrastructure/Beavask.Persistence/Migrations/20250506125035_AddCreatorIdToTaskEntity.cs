using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beavask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIdToTaskEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Tasks",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Tasks");
        }
    }
}
