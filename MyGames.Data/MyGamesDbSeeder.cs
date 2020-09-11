using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyGames.Data.Constants;
using MyGames.Data.Contexts;
using MyGames.Data.Models;
using MyGames.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGames.Data
{
    public class MyGamesDbSeeder
    {
        public static async Task Initialize(MyGamesDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if (!context.Roles.Any())
            {
                await CreateRolesAsync(context, roleManager);
            }

            if (!context.TipoPessoas.Any())
            {
                await CreateTipoPessoaAsync(context);
            }

            if (!context.Users.Any())
            {
                await CreateUsersAsync(context, userManager);
            }

            if (!context.TipoJogos.Any())
            {
                await CreateTipoJogoAsync(context);
            }
        }

        private static async Task CreateRolesAsync(MyGamesDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            await roleManager.CreateAsync(new ApplicationRole() { Name = AccountConstants.Administrador, NormalizedName = AccountConstants.Administrador });
            await roleManager.CreateAsync(new ApplicationRole() { Name = AccountConstants.Amigo, NormalizedName = AccountConstants.Amigo });
        }

        private static async Task CreateUsersAsync(MyGamesDbContext context, UserManager<ApplicationUser> userManager)
        {
            var user = new ApplicationUser
            {
                UserName = AccountConstants.AdminUser,
                Email = "admin@mygames.com.br",
                EmailConfirmed = true,
                Status = true,
                Pessoa = new Pessoa
                {
                    Nome = "Gleydson Guede Morais",
                    TipoPessoaId = AccountConstants.AdministradorDB,
                    DtCadastro = DateTime.Now
                }
            };

            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, AccountConstants.Administrador);
        }

        private static async Task CreateTipoPessoaAsync(MyGamesDbContext context)
        {
            var tipoPessoa = new List<TipoPessoa>
            {
                new TipoPessoa{ Descricao = AccountConstants.Administrador },
                new TipoPessoa{ Descricao = AccountConstants.Amigo }
            };

            context.AddRange(tipoPessoa);
            await context.SaveChangesAsync();
        }

        private static async Task CreateTipoJogoAsync(MyGamesDbContext context)
        {
            var tipoPessoa = new List<TipoPessoa>
            {
                new TipoPessoa{ Descricao = GamesConstants.Tabuleiro },
                new TipoPessoa{ Descricao = GamesConstants.Cartas },
                new TipoPessoa{ Descricao = GamesConstants.PS4 },
                new TipoPessoa{ Descricao = GamesConstants.XBox }
            };

            context.AddRange(tipoPessoa);
            await context.SaveChangesAsync();
        }
    }
}
