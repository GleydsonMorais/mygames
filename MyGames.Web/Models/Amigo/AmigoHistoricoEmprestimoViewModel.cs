using MyGames.Web.Models.Jogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Amigo
{
    public class AmigoHistoricoEmprestimoViewModel
    {
        public string Nome { get; set; }

        public IList<HistoricoEmprestimoViewModel> Historico { get; set; }
    }

    public class HistoricoEmprestimoViewModel
    {
        public string Nome { get; set; }
        public string TipoJogo { get; set; }
        public string DtEmprestimo { get; set; }
        public string DtDevolucao { get; set; }
    }
}
