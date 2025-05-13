using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beavask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyPropToLogEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Logs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CompanyId",
                table: "Logs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Companies_CompanyId",
                table: "Logs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Companies_CompanyId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_CompanyId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Logs");
        }
    }
}
