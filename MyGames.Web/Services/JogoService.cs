using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyGames.Data.Constants;
using MyGames.Data.Helpers;
using MyGames.Object.Jogo;
using MyGames.Web.Interfaces;
using MyGames.Web.Models.Config;
using MyGames.Web.Models.Jogo;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Services
{
    public class JogoService : IJogoService
    {
        private readonly MyGamesAPIConfig _myGamesAPIConfig;
        private readonly ILookupService _lookupService;

        public JogoService(IOptions<MyGamesAPIConfig> myGamesAPIConfig,
            ILookupService lookupService)
        {
            _myGamesAPIConfig = myGamesAPIConfig.Value;
            _lookupService = lookupService;
        }

        public async Task<JogoResult> GetJogoAsync(int id)
        {
            var jogo = new JogoResult();

            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/jogo/{id}", Method.GET);
            request.AddParameter("id", id, ParameterType.UrlSegment);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<JogoResult> response = await client.ExecuteAsync<JogoResult>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                jogo = response.Data;
            }

            return jogo;
        }

        public async Task<IList<JogoIndexViewModel>> ListJogoAsync([FromQuery]JogoFilterViewModel filter)
        {
            var listAmigo = new List<JogoIndexViewModel>();

            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/jogo", Method.GET);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<List<JogoResult>> response = await client.ExecuteAsync<List<JogoResult>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = response.Data;

                if (!string.IsNullOrEmpty(filter.Nome))
                    result = result.Where(x => x.Nome.ToUpper().Contains(filter.Nome.ToUpper())).ToList();

                if (filter.TipoJogoId.HasValue)
                    result = result.Where(x => x.TipoJogoId == filter.TipoJogoId).ToList();

                if (filter.Emprestado.HasValue)
                    result = result.Where(x => x.Emprestado == filter.Emprestado).ToList();

                listAmigo = result.Select(x =>
                new JogoIndexViewModel
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    TipoJogo = x.Tipo.Descricao,
                    Emprestado = GamesConstants.GetStatusDevolucao(x.Emprestado),
                    Amigo = x.Historico.SingleOrDefault(y => y.Devolvido == false)?.Amigo.Nome,
                    DtEmprestimo = x.Historico.SingleOrDefault(y => y.Devolvido == false)?.DtEmprestimo.ToShortDateString(),
                }).ToList();
            }

            return listAmigo;
        }

        public async Task<JogoHistoricoViewModel> GetJogoHistoricoAsync(int id)
        {
            var jogo = await GetJogoAsync(id);
            return new JogoHistoricoViewModel
            {
                Nome = jogo.Nome,
                Tipo = jogo.Tipo.Descricao,
                Historico = jogo.Historico.Select(x =>
                new HistoricoEmprestimoViewModel
                {
                    Amigo = x.Amigo.Nome,
                    DtEmprestimo = x.DtEmprestimo.ToShortDateString(),
                    DtDevolucao = x.DtDevolucao.HasValue ? x.DtDevolucao.Value.ToShortDateString() : null
                }).ToList()
            };
        }

        public async Task<QueryResult<JogoCreateViewModel>> CreateJogoAsync(JogoCreateViewModel model)
        {
            var jogo = new JogoResult();

            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/jogo", Method.POST);
            request.AddJsonBody(model);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<string> response = await client.ExecuteAsync<string>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return new QueryResult<JogoCreateViewModel>
                {
                    Succeeded = true,
                    Result = model,
                    Message = response.Data
                };
            }

            return new QueryResult<JogoCreateViewModel>
            {
                Succeeded = false,
                Result = model,
                Message = response.Data
            };
        }

        public async Task<JogoEditViewModel> GetJogoEditAsync(int id)
        {
            var jogo = await GetJogoAsync(id);
            return new JogoEditViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Tipo = jogo.Tipo.Descricao,
                Emprestado = GamesConstants.GetStatusDevolucao(jogo.Emprestado),
                Amigo = jogo.Historico.SingleOrDefault(y => y.Devolvido == false)?.Amigo.Nome,
                DtEmprestimo = jogo.Historico.SingleOrDefault(y => y.Devolvido == false)?.DtEmprestimo.ToShortDateString()
            };
        }

        public async Task<QueryResult<JogoEditViewModel>> EditJogoAsync(JogoEditViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/jogo/{id}", Method.PUT);
            request.AddParameter("id", model.Id, ParameterType.UrlSegment);
            request.AddParameter("nome", model.Nome, ParameterType.QueryString);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<string> response = await client.ExecuteAsync<string>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new QueryResult<JogoEditViewModel>
                {
                    Succeeded = true,
                    Result = model,
                    Message = "Jogo editado com sucesso."
                };
            }

            return new QueryResult<JogoEditViewModel>
            {
                Succeeded = false,
                Result = model,
                Message = response.Data
            };
        }

        public async Task<QueryResult<string>> DevolverJogoAsync(int id)
        {
            try
            {
                RestClient client = new RestClient(_myGamesAPIConfig.URL);
                RestRequest request = new RestRequest("api/jogo/devolver/{id}", Method.PUT);
                request.AddParameter("id", id, ParameterType.UrlSegment);

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

                IRestResponse<string> response = await client.ExecuteAsync<string>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new QueryResult<string>
                    {
                        Succeeded = true,
                        Message = "Jogo devolvido com sucesso."
                    };
                }

                return new QueryResult<string>
                {
                    Succeeded = false,
                    Message = response.Data
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

        public async Task<QueryResult<string>> EmprestarJogoAsync(int id, int amigoId)
        {
            try
            {
                RestClient client = new RestClient(_myGamesAPIConfig.URL);
                RestRequest request = new RestRequest("api/jogo/emprestar/{id}/{amigoId}", Method.PUT);
                request.AddParameter("id", id, ParameterType.UrlSegment);
                request.AddParameter("amigoId", amigoId, ParameterType.UrlSegment);

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

                IRestResponse<string> response = await client.ExecuteAsync<string>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new QueryResult<string>
                    {
                        Succeeded = true,
                        Message = "Jogo emprestado com sucesso."
                    };
                }

                return new QueryResult<string>
                {
                    Succeeded = false,
                    Message = response.Data
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

        public async Task<JogoDeleteViewModel> GetJogoDeleteAsync(int id)
        {
            var jogo = await GetJogoAsync(id);
            return new JogoDeleteViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Tipo = jogo.Tipo.Descricao,
                Emprestado = GamesConstants.GetStatusDevolucao(jogo.Emprestado),
                Amigo = jogo.Historico.SingleOrDefault(y => y.Devolvido == false)?.Amigo.Nome,
                DtEmprestimo = jogo.Historico.SingleOrDefault(y => y.Devolvido == false)?.DtEmprestimo.ToShortDateString(),
                Historico = jogo.Historico.Where(x => x.Devolvido == true).Select(x =>
                new HistoricoEmprestimoViewModel
                {
                    Amigo = x.Amigo.Nome,
                    DtEmprestimo = x.DtEmprestimo.ToShortDateString(),
                    DtDevolucao = x.DtDevolucao.Value.ToShortDateString()
                }).ToList()
            };
        }

        public async Task<QueryResult<string>> DeleteJogoAsync(JogoDeleteViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/jogo/{id}", Method.DELETE);
            request.AddParameter("id", model.Id, ParameterType.UrlSegment);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<string> response = await client.ExecuteAsync<string>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new QueryResult<string>
                {
                    Succeeded = true,
                    Message = "Jogo deletado com sucesso."
                };
            }

            return new QueryResult<string>
            {
                Succeeded = false,
                Message = response.Data
            };
        }
    }
}
