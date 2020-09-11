using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Models
{
    public class TipoJogo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public IList<Jogo> Jogos { get; set; } = new List<Jogo>();
    }
}
