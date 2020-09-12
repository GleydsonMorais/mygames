using MyGames.Object.Jogo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Object.Amigo
{
    public class Amigo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool JogoEmprestado { get; set; }
        public string Status { get; set; }

        public IList<JogoEmprestado> HistoricoEmprestimo { get; set; }
    }
}
