using Microsoft.EntityFrameworkCore.Migrations;

namespace MyGames.Data.Migrations
{
    public partial class alteracao_OnDelete_Pessoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoa_Users_LoginId",
                table: "Pessoa");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoa_Users_LoginId",
                table: "Pessoa",
                column: "LoginId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoa_Users_LoginId",
                table: "Pessoa");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoa_Users_LoginId",
                table: "Pessoa",
                column: "LoginId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
