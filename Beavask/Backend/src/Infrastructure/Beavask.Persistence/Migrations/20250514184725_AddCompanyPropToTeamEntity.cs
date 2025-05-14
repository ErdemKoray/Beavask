using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beavask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyPropToTeamEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Teams",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CompanyId",
                table: "Teams",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Companies_CompanyId",
                table: "Teams",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Companies_CompanyId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CompanyId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Teams");
        }
    }
}
