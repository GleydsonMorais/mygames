using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGames.API.Interfaces;
using MyGames.API.Models.Amigo;
using MyGames.Data.Constants;
using MyGames.Data.Contexts;
using MyGames.Data.Helpers;
using MyGames.Data.Models.Identity;
using MyGames.Object.Amigo;
using MyGames.Object.Jogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Services
{
    public class AmigoService : IAmigoService
    {
        public readonly MyGamesDbContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AmigoService(MyGamesDbContext dataContext,
            UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<Amigo> GetAmigoAsync(int id)
        {
            var amigo = await _dataContext.Pessoas
                .Include(x => x.TipoPessoa)
                .Include(x => x.Login)
                .Include(x => x.JogosEmprestados).ThenInclude(x => x.Jogo).ThenInclude(x => x.TipoJogo)
                .SingleOrDefaultAsync(x => x.Id == id);

            return new Amigo
            {
                Id = amigo.Id,
                UserName = amigo.Login.UserName,
                Nome = amigo.Nome,
                TipoPessoa = amigo.TipoPessoa.Descricao,
                Telefone = amigo.Telefone,
                Email = amigo.Login.Email,
                JogoEmprestado = amigo.JogosEmprestados.Count() > 0 ? true : false,
                HistoricoEmprestimo = amigo.JogosEmprestados.Select(x =>
                new JogoEmprestado
                {
                    JogoId = x.JogoId,
                    Nome = x.Jogo.Nome,
                    TipoJogo = x.Jogo.TipoJogo.Descricao,
                    DtEmprestimo = x.DtEmprestimo.ToShortDateString(),
                    DtDevolucao = x.DtDevolucao.HasValue ? x.DtDevolucao.Value.ToShortDateString() : null,
                    Devolvido = GamesConstants.GetStatusDevolucao(x.Devolvido)
                }).ToList(),
                Status = AccountConstants.GetStatusUsuario(amigo.Login.Status)
            };
        }

        public async Task<IList<Amigo>> GetListAmigoAsync() => await _dataContext.Pessoas
            .Include(x => x.TipoPessoa)
            .Include(x => x.Login)
            .Include(x => x.JogosEmprestados)
            .Where(x => x.TipoPessoaId == AccountConstants.AmigoDB)
            .Select(x =>
            new Amigo
            {
                Id = x.Id,
                UserName = x.Login.UserName,
                Nome = x.Nome,
                TipoPessoa = x.TipoPessoa.Descricao,
                Telefone = x.Telefone,
                Email = x.Login.Email,
                JogoEmprestado = x.JogosEmprestados.Count() > 0 ? true : false,
                Status = AccountConstants.GetStatusUsuario(x.Login.Status)
            }).ToListAsync();

        public async Task<QueryResult<string>> CreateAmigoAsync([FromBody]AmigoCreate model)
        {
            try
            {
                var emailCadastrado = await _dataContext.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());
                if (emailCadastrado != null)
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "E-mail já cadastrado!"
                    };
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    Status = true,
                    Pessoa = new MyGames.Data.Models.Pessoa
                    {
                        Nome = model.Nome,
                        TipoPessoaId = AccountConstants.AmigoDB,
                        DtCadastro = DateTime.Now
                    }
                };

                await _userManager.CreateAsync(user, "###123321###");
                await _userManager.AddToRoleAsync(user, AccountConstants.Amigo);

                return new QueryResult<string>
                {
                    Succeeded = true,
                    Message = "Amigo cadastrado com sucesso."
                };
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

        public async Task<QueryResult<string>> EditAmigoAsync(int id, [FromBody]AmigoEdit model)
        {
            try
            {
                var pessoa = await _dataContext.Pessoas
                    .Include(x => x.Login)
                    .SingleOrDefaultAsync(x => x.Id == id);
                if (pessoa != null)
                {
                    var emailDiferente = (pessoa.Login.Email.ToUpper() != model.Email.ToUpper());
                    var emailCadastrado = await _dataContext.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());
                    if (emailDiferente && emailCadastrado != null)
                    {
                        return new QueryResult<string>
                        {
                            Succeeded = false,
                            Message = "E-mail já cadastrado!"
                        };
                    }

                    pessoa.Nome = model.Nome;
                    pessoa.Telefone = model.Telefone;
                    pessoa.Login.Email = model.Email.ToLower();
                    pessoa.Login.NormalizedEmail = model.Email.ToUpper();
                    pessoa.Login.Status = AccountConstants.GetStatusDbUsuario(model.Status);

                    _dataContext.Update(pessoa);
                    await _dataContext.SaveChangesAsync();

                    return new QueryResult<string>
                    {
                        Succeeded = true,
                        Message = "Amigo editado com sucesso."
                    };
                }
                else
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "Erro, amigo não encontrado!"
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

        public async Task<QueryResult<string>> DeleteAmigoAsync(int id)
        {
            try
            {
                var amigo = await _dataContext.Pessoas
                    .Include(x => x.TipoPessoa)
                    .Include(x => x.Login)
                    .Include(x => x.JogosEmprestados).ThenInclude(x => x.Jogo).ThenInclude(x => x.TipoJogo)
                    .SingleOrDefaultAsync(x => x.Id == id);
                if (amigo.JogosEmprestados.Where(x => x.Devolvido == false).Count() == 0)
                {
                    _dataContext.Remove(amigo);
                    _dataContext.Remove(amigo.Login);
                    await _dataContext.SaveChangesAsync();

                    return new QueryResult<string>
                    {
                        Succeeded = true,
                        Message = "Amigo deletado com sucesso."
                    };
                }
                else
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "Amigo não pode ser deletado, pois ainda está com um ou mais jogos emprestados!"
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
