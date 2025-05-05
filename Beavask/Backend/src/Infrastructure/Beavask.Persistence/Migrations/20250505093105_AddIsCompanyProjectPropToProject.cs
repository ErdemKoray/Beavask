using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beavask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompanyProjectPropToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyProject",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompanyProject",
                table: "Projects");
        }
    }
}
