using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyGames.Data.Migrations
{
    public partial class alter_de_JogoEmprestado_para_HistoricoEmprestimo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JogoEmprestado");

            migrationBuilder.CreateTable(
                name: "HistoricoEmprestimo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PessoaId = table.Column<int>(nullable: false),
                    JogoId = table.Column<int>(nullable: false),
                    DtEmprestimo = table.Column<DateTime>(nullable: false),
                    DtDevolucao = table.Column<DateTime>(nullable: true),
                    Devolvido = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoEmprestimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoEmprestimo_Jogo_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoEmprestimo_Pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEmprestimo_JogoId",
                table: "HistoricoEmprestimo",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEmprestimo_PessoaId",
                table: "HistoricoEmprestimo",
                column: "PessoaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoEmprestimo");

            migrationBuilder.CreateTable(
                name: "JogoEmprestado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Devolvido = table.Column<bool>(nullable: false),
                    DtDevolucao = table.Column<DateTime>(nullable: true),
                    DtEmprestimo = table.Column<DateTime>(nullable: false),
                    JogoId = table.Column<int>(nullable: false),
                    PessoaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JogoEmprestado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JogoEmprestado_Jogo_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JogoEmprestado_Pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JogoEmprestado_JogoId",
                table: "JogoEmprestado",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoEmprestado_PessoaId",
                table: "JogoEmprestado",
                column: "PessoaId");
        }
    }
}
