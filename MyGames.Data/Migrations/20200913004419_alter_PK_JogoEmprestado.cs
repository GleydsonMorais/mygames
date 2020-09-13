using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyGames.Data.Migrations
{
    public partial class alter_PK_JogoEmprestado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JogoEmprestado",
                table: "JogoEmprestado");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "JogoEmprestado",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JogoEmprestado",
                table: "JogoEmprestado",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_JogoEmprestado_PessoaId",
                table: "JogoEmprestado",
                column: "PessoaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JogoEmprestado",
                table: "JogoEmprestado");

            migrationBuilder.DropIndex(
                name: "IX_JogoEmprestado_PessoaId",
                table: "JogoEmprestado");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "JogoEmprestado");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JogoEmprestado",
                table: "JogoEmprestado",
                columns: new[] { "PessoaId", "JogoId" });
        }
    }
}
