using MyGames.Web.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Amigo
{
    public class AmigoFilterViewModel : FilterViewModel
    {
        public string Nome { get; set; }
        public string Status { get; set; }
        public bool Pesquisa { get; set; }
    }

    public class AmigoIndexViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool JogoEmpresatado { get; set; }
        public string Status { get; set; }
    }
}
