using MyGames.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TipoPessoaId { get; set; }
        public string Telefone { get; set; }
        public DateTime DtCadastro { get; set; }
        public string LoginId { get; set; }

        public ApplicationUser Login { get; set; }
        public TipoPessoa TipoPessoa { get; set; }

        public IList<HistoricoEmprestimo> HistoricoEmprestimos { get; set; } = new List<HistoricoEmprestimo>();
    }
}
