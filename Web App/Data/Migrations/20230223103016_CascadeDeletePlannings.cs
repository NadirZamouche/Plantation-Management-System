using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Data.Migrations
{
    public partial class CascadeDeletePlannings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plannings_Users_UserId",
                table: "Plannings");

            migrationBuilder.AddForeignKey(
                name: "FK_Plannings_Users_UserId",
                table: "Plannings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plannings_Users_UserId",
                table: "Plannings");

            migrationBuilder.AddForeignKey(
                name: "FK_Plannings_Users_UserId",
                table: "Plannings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
