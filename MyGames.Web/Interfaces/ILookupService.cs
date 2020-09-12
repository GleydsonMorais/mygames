using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyGames.Web.Interfaces
{
    public interface ILookupService
    {
        Task<string> GetNomeUsuarioLogadoAsync(ClaimsPrincipal user);
        Task<IList<SelectListItem>> SelectListTipoJogoAsync();
        Task<IList<SelectListItem>> SelectListAmigAsync();
    }
}
