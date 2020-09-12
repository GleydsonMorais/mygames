using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Object.Jogo
{
    public class JogoResult
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TipoJogoId { get; set; }
        public bool Emprestado { get; set; }

        public TipoJogoResult Tipo { get; set; }

        public IList<HistoricoEmprestimoResult> Historico { get; set; }
    }
}
