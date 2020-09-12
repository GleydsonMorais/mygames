using MyGames.Data.Helpers;
using MyGames.Web.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Interfaces
{
    public interface IIdentityService
    {
        Task<QueryResult<string>> LoginAsync(LoginModel.InputModel model);
    }
}
