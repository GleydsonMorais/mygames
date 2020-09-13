using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Models
{
    public class JogoEmprestado
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public int JogoId { get; set; }
        public DateTime DtEmprestimo { get; set; }
        public DateTime? DtDevolucao { get; set; }
        public bool Devolvido { get; set; }

        public Pessoa Pessoa { get; set; }
        public Jogo Jogo { get; set; }
    }
}
