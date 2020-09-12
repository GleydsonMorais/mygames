using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Jogo
{
    public class JogoCreateViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int TipoJogoId { get; set; }
    }
}
