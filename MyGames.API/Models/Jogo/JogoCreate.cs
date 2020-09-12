using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Models.Jogo
{
    public class JogoCreate
    {
        public string Nome { get; set; }
        public int TipoJogoId { get; set; }
    }
}
