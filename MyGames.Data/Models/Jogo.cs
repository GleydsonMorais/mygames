using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TipoJogoId { get; set; }
        public bool Emprestado { get; set; }

        public TipoJogo TipoJogo { get; set; }

        public IList<HistoricoEmprestimo> HistoricoEmprestimo { get; set; } = new List<HistoricoEmprestimo>();
    }
}
