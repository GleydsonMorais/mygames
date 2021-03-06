﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Jogo
{
    public class JogoDeleteViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Emprestado { get; set; }
        public string Amigo { get; set; }
        public string DtEmprestimo { get; set; }

        public IList<HistoricoEmprestimoViewModel> Historico { get; set; }
    }
}
