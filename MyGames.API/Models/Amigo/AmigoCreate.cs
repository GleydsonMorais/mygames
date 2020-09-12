using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Models.Amigo
{
    public class AmigoCreate
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Telefone { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
