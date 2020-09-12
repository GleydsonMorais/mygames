using MyGames.Object.Amigo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Object.Jogo
{
    public class HistoricoEmprestimoResult
    {
        public int PessoaId { get; set; }
        public int JogoId { get; set; }
        public DateTime DtEmprestimo { get; set; }
        public DateTime? DtDevolucao { get; set; }
        public bool Devolvido { get; set; }

        public AmigoResult Amigo { get; set; }
    }
}
