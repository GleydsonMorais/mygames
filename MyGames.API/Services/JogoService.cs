using Microsoft.AspNetCore.Mvc;
using MyGames.API.Interfaces;
using MyGames.API.Models.Jogo;
using MyGames.Data.Helpers;
using MyGames.Data.Models;
using MyGames.Data.Repositories;
using MyGames.Object.Amigo;
using MyGames.Object.Jogo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IAmigoRepository _amigoRepository;

        public JogoService(IJogoRepository jogoRepository,
            IAmigoRepository amigoRepository)
        {
            _jogoRepository = jogoRepository;
            _amigoRepository = amigoRepository;
        }

        public async Task<IList<JogoResult>> GetListJogoAsync()
        {
            var listJogo = await _jogoRepository.AllAsync();
            return listJogo.Select(x =>
            new JogoResult
            {
                Id = x.Id,
                Nome = x.Nome,
                TipoJogoId = x.TipoJogoId,
                Emprestado = x.Emprestado,
                Tipo = new TipoJogoResult
                {
                    Id = x.TipoJogo.Id,
                    Descricao = x.TipoJogo.Descricao
                },
                Historico = x.HistoricoEmprestimo.Select(y =>
                new HistoricoEmprestimoResult
                {
                    PessoaId = y.PessoaId,
                    JogoId = y.JogoId,
                    DtDevolucao = y.DtDevolucao,
                    DtEmprestimo = y.DtEmprestimo,
                    Devolvido = y.Devolvido,
                    Amigo = new AmigoResult
                    {
                        Id = y.Pessoa.Id,
                        Nome = y.Pessoa.Nome,
                        Telefone = y.Pessoa.Telefone,
                        Email = y.Pessoa.Login.Email
                    }
                }).ToList()
            }).ToList();
        }

        public async Task<JogoResult> GetJogoAsync(int id)
        {
            var jogo = await _jogoRepository.GetJogoAsync(id);
            if (jogo != null)
            {
                return new JogoResult
                {
                    Id = jogo.Id,
                    Nome = jogo.Nome,
                    TipoJogoId = jogo.TipoJogoId,
                    Emprestado = jogo.Emprestado,
                    Tipo = new TipoJogoResult
                    {
                        Id = jogo.TipoJogo.Id,
                        Descricao = jogo.TipoJogo.Descricao
                    },
                    Historico = jogo.HistoricoEmprestimo.Select(x =>
                    new HistoricoEmprestimoResult
                    {
                        PessoaId = x.PessoaId,
                        JogoId = x.JogoId,
                        DtDevolucao = x.DtDevolucao,
                        DtEmprestimo = x.DtEmprestimo,
                        Devolvido = x.Devolvido,
                        Amigo = new AmigoResult
                        {
                            Id = x.Pessoa.Id,
                            Nome = x.Pessoa.Nome,
                            Telefone = x.Pessoa.Telefone,
                            Email = x.Pessoa.Login.Email
                        }
                    }).ToList()
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<QueryResult<string>> CreateJogoAsync(JogoCreate model)
        {
            try
            {
                var tipoJogo = await _jogoRepository.GetTipoJogoAsync(model.TipoJogoId);
                if (tipoJogo != null)
                {
                    var jogo = await _jogoRepository.ValidateGameAsync(model.Nome, model.TipoJogoId);
                    if (jogo == null)
                    {
                        var jogoDb = new Jogo
                        {
                            Nome = model.Nome,
                            TipoJogoId = model.TipoJogoId
                        };

                        await _jogoRepository.InsertJogoAsync(jogoDb);

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
                            Message = "Jogo já cadastrado!"
                        };
                    }
                }
                else
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "Tipo do Jogo não esta cadastrado!"
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

        public async Task<QueryResult<string>> UpdateJogoAsync(int id, string nome)
        {
            try
            {
                var jogo = await _jogoRepository.GetJogoAsync(id);
                if (jogo != null)
                {
                    var validateJogo = await _jogoRepository.ValidateGameAsync(nome, jogo.TipoJogoId);
                    if ((nome.ToLower() == jogo.Nome.ToLower()) || ((nome.ToLower() != jogo.Nome.ToLower()) && validateJogo == null))
                    {
                        //if (model.AmigoId.HasValue)
                        //{
                        //    var amigo = await _amigoRepository.GetAmigoAsync(model.AmigoId.Value);
                        //    if (amigo != null)
                        //    {
                        //        if (!jogo.Emprestado)
                        //        {
                        //            jogo.Emprestado = true;
                        //            jogo.HistoricoEmprestimo.Add(new MyGames.Data.Models.JogoEmprestado
                        //            {
                        //                PessoaId = amigo.Id,
                        //                DtEmprestimo = DateTime.Now,
                        //                Devolvido = false
                        //            });

                        //            await _jogoRepository.UpdateJogoAsync(jogo);

                        //            return new QueryResult<string>
                        //            {
                        //                Succeeded = true,
                        //                Message = "Jogo emprestado com sucesso."
                        //            };
                        //        }
                        //        else
                        //        {
                        //            return new QueryResult<string>
                        //            {
                        //                Succeeded = false,
                        //                Message = "Jogo já está emprestado!"
                        //            };
                        //        }
                        //    }
                        //    else
                        //    {
                        //        return new QueryResult<string>
                        //        {
                        //            Succeeded = false,
                        //            Message = "Amigo não esta cadastrado!"
                        //        };
                        //    }
                        //}

                        jogo.Nome = nome;

                        await _jogoRepository.UpdateJogoAsync(jogo);

                        return new QueryResult<string>
                        {
                            Succeeded = true,
                            Message = "Jogo editado com sucesso."
                        };
                    }
                    else
                    {
                        return new QueryResult<string>
                        {
                            Succeeded = false,
                            Message = "Jogo já existe!"
                        };
                    }
                }
                else
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "Jogo não cadastrado!"
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

        public async Task<QueryResult<string>> DevolverJogoAsync(int id)
        {
            try
            {
                var jogo = await _jogoRepository.GetJogoAsync(id);
                if (jogo != null)
                {
                    if (jogo.Emprestado)
                    {
                        jogo.Emprestado = false;

                        var historico = jogo.HistoricoEmprestimo.First(x => x.Devolvido == false);
                        historico.DtDevolucao = DateTime.Now;
                        historico.Devolvido = true;

                        await _jogoRepository.UpdateJogoAsync(jogo);

                        return new QueryResult<string>
                        {
                            Succeeded = true,
                            Message = "Jogo devolvido com sucesso!"
                        };
                    }
                    else
                    {
                        return new QueryResult<string>
                        {
                            Succeeded = false,
                            Message = "Jogo não está emprestado!"
                        };
                    }
                }
                else
                {
                    return new QueryResult<string>
                    {
                        Succeeded = false,
                        Message = "Jogo não cadastrado!"
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
