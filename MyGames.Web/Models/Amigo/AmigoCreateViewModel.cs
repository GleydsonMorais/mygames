using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Models.Amigo
{
    public class AmigoCreateViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string UserName { get; set; }

        public string Telefone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Email { get; set; }
    }
}
