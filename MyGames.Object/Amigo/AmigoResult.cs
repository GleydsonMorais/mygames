using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Object.Amigo
{
    public class AmigoResult
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nome { get; set; }
        public int TipoPessoaId { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool JogoEmprestado { get; set; }
        public bool Status { get; set; }

        public IList<HistoricoEmprestimoResult> HistoricoEmprestimo { get; set; }
    }

    public class HistoricoEmprestimoResult
    {
        public int JogoId { get; set; }
        public string Nome { get; set; }
        public string TipoJogo { get; set; }
        public DateTime DtEmprestimo { get; set; }
        public DateTime? DtDevolucao { get; set; }
        public bool Devolvido { get; set; }
    }
}
