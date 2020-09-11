using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public bool Status { get; set; }
        public Pessoa Pessoa { get; set; }

        public virtual ICollection<ApplicationUserRole> Roles { get; } = new List<ApplicationUserRole>();
    }
}
