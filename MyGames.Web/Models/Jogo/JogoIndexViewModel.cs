using Microsoft.AspNetCore.Mvc.Rendering;
using MyGames.Web.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Jogo
{
    public class JogoFilterViewModel : FilterViewModel
    {
        public string Nome { get; set; }
        public int? TipoJogoId { get; set; }
        public bool? Emprestado { get; set; }
        public bool Pesquisa { get; set; }
    }

    public class JogoIndexViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TipoJogo { get; set; }
        public string Emprestado { get; set; }
        public string Amigo { get; set; }
        public string DtEmprestimo { get; set; }
    }
}
