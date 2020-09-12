using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Object.Jogo
{
    public class JogoEmprestado
    {
        public int JogoId { get; set; }
        public string Nome { get; set; }
        public string TipoJogo { get; set; }
        public string DtEmprestimo { get; set; }
        public string DtDevolucao { get; set; }
        public string Devolvido { get; set; }
    }
}
