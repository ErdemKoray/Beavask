using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beavask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIdToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Projects",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                newName: "IX_Projects_UserId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompanyId",
                table: "Projects",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_UserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CompanyId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Projects",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                newName: "IX_Projects_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
