using MyGames.Web.Models.Jogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Amigo
{
    public class AmigoDeleteViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UserName { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public IList<JogoEmprestado> JogosEmprestados { get; set; }
    }
}
