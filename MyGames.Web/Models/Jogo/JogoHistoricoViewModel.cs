using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Jogo
{
    public class JogoHistoricoViewModel
    {
        public string Nome { get; set; }
        public string Tipo { get; set; }

        public IList<HistoricoEmprestimoViewModel> Historico { get; set; }
    }

    public class HistoricoEmprestimoViewModel
    {
        public string Amigo { get; set; }
        public string DtEmprestimo { get; set; }
        public string DtDevolucao { get; set; }
    }
}
