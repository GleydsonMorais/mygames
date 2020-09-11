using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Models
{
    public class TipoPessoa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public IList<Pessoa> Pessoas { get; set; } = new List<Pessoa>();
    }
}
