﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Object.Jogo
{
    public class JogoResult
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TipoJogoId { get; set; }
        public bool Emprestado { get; set; }

        public TipoJogoResult Tipo { get; set; }

        public IList<HistoricoEmprestimoResult> Historico { get; set; }
    }

    public class TipoJogoResult
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }

    public class HistoricoEmprestimoResult
    {
        public int PessoaId { get; set; }
        public int JogoId { get; set; }
        public DateTime DtEmprestimo { get; set; }
        public DateTime? DtDevolucao { get; set; }
        public bool Devolvido { get; set; }

        public AmigoResult Amigo { get; set; }
    }

    public class AmigoResult
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}
