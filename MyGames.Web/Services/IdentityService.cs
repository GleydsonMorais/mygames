using Microsoft.AspNetCore.Identity;
using MyGames.Data.Helpers;
using MyGames.Data.Models.Identity;
using MyGames.Web.Areas.Identity.Pages.Account;
using MyGames.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService( UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<QueryResult<string>> LoginAsync(LoginModel.InputModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Login);
                if (user.Status)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return new QueryResult<string>
                        {
                            Succeeded = true
                        };
                    }
                    else
                    {
                        return new QueryResult<string>
                        {
                            Succeeded = false,
                            Message = "Erro, login ou senha invalio!"
                        };
                    }
                }
                else
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "Erro, usuario inativo!"
                    };
                }
            }
            catch (Exception)
            {
                return new QueryResult<string>
                {
                    Succeeded = false,
                    Message = "Erro, tente novamente!"
                };
            }
        }
    }
}
