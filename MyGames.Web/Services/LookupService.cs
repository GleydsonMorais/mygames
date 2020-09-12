﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyGames.Data.Contexts;
using MyGames.Data.Models.Identity;
using MyGames.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyGames.Web.Services
{
    public class LookupService : ILookupService
    {
        private readonly MyGamesDbContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public LookupService(MyGamesDbContext dataContext,
            UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<string> GetIdUsuarioLogadoAsync(ClaimsPrincipal user)
        {
            var userDb = await _userManager.GetUserAsync(user);
            return userDb.Id;
        }

        public async Task<string> GetNomeUsuarioLogadoAsync(ClaimsPrincipal user)
        {
            var userId = await GetIdUsuarioLogadoAsync(user);
            var userDb = await _dataContext.Pessoas.SingleOrDefaultAsync(x => x.LoginId == userId);
            return userDb.Nome;
        }
    }
}
