using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGames.API.Interfaces;
using MyGames.API.Models.Amigo;
using MyGames.Data.Constants;
using MyGames.Data.Helpers;
using MyGames.Data.Models.Identity;
using MyGames.Data.Repositories;
using MyGames.Object.Amigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Services
{
    public class AmigoService : IAmigoService
    {
        private readonly IAmigoRepository _amigoRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AmigoService(IAmigoRepository amigoRepository,
            UserManager<ApplicationUser> userManager)
        {
            _amigoRepository = amigoRepository;
            _userManager = userManager;
        }

        public async Task<IList<AmigoResult>> GetListAmigoAsync()
        {
            var listAmigo = await _amigoRepository.AllAsync();
            return listAmigo.Select(x =>
            new AmigoResult
            {
                Id = x.Id,
                UserName = x.Login.UserName,
                Nome = x.Nome,
                TipoPessoaId = x.TipoPessoaId,
                Telefone = x.Telefone,
                Email = x.Login.Email,
                JogoEmprestado = x.HistoricoEmprestimos.Count() > 0 ? true : false,
                Status = x.Login.Status
            }).ToList();
        }

        public async Task<AmigoResult> GetAmigoAsync(int id)
        {
            var amigo = await _amigoRepository.GetAmigoAsync(id);
            if (amigo != null)
            {
                return new AmigoResult
                {
                    Id = amigo.Id,
                    UserName = amigo.Login.UserName,
                    Nome = amigo.Nome,
                    TipoPessoaId = amigo.TipoPessoaId,
                    Telefone = amigo.Telefone,
                    Email = amigo.Login.Email,
                    JogoEmprestado = amigo.HistoricoEmprestimos.Count() > 0 ? true : false,
                    HistoricoEmprestimo = amigo.HistoricoEmprestimos.Select(x =>
                    new HistoricoEmprestimoResult
                    {
                        JogoId = x.JogoId,
                        Nome = x.Jogo.Nome,
                        TipoJogo = x.Jogo.TipoJogo.Descricao,
                        DtEmprestimo = x.DtEmprestimo,
                        DtDevolucao = x.DtDevolucao,
                        Devolvido = x.Devolvido
                    }).ToList(),
                    Status = amigo.Login.Status
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<QueryResult<string>> CreateAmigoAsync([FromBody]AmigoCreate model)
        {
            try
            {
                var emailCadastrado = await _amigoRepository.ValidateEmialAsync(model.Email);
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

        public async Task<QueryResult<string>> UpdateAmigoAsync(int id, [FromBody]AmigoEdit model)
        {
            try
            {
                var amigo = await _amigoRepository.GetAmigoAsync(id);
                if (amigo != null)
                {
                    var emailDiferente = (amigo.Login.Email.ToUpper() != model.Email.ToUpper());
                    var emailCadastrado = await _amigoRepository.ValidateEmialAsync(model.Email);
                    if (emailDiferente && emailCadastrado != null)
                    {
                        return new QueryResult<string>
                        {
                            Succeeded = false,
                            Message = "E-mail já cadastrado!"
                        };
                    }

                    amigo.Nome = model.Nome;
                    amigo.Telefone = model.Telefone;
                    amigo.Login.Email = model.Email.ToLower();
                    amigo.Login.NormalizedEmail = model.Email.ToUpper();
                    amigo.Login.Status = AccountConstants.GetStatusDbUsuario(model.Status);

                    await _amigoRepository.UpdateAmigoAsync(amigo);

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
                var amigo = await _amigoRepository.GetAmigoAsync(id);
                if (amigo.HistoricoEmprestimos.Where(x => x.Devolvido == false).Count() == 0)
                {
                    await _amigoRepository.DeleteAmigoAsync(amigo);

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
