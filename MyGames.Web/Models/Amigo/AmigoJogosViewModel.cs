using MyGames.Web.Models.Jogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Amigo
{
    public class AmigoJogosViewModel
    {
        public string Nome { get; set; }

        public IList<JogoEmprestado> JogosEmprestados { get; set; }
    }
}
