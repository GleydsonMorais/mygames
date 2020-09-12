using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyGames.Data.Constants;
using MyGames.Data.Helpers;
using MyGames.Object.Amigo;
using MyGames.Web.Interfaces;
using MyGames.Web.Models.Amigo;
using MyGames.Web.Models.Config;
using MyGames.Web.Models.Jogo;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Services
{
    public class AmigoService : IAmigoService
    {
        private readonly MyGamesAPIConfig _myGamesAPIConfig;
        private readonly ILookupService _lookupService;

        public AmigoService(IOptions<MyGamesAPIConfig> myGamesAPIConfig,
            ILookupService lookupService)
        {
            _myGamesAPIConfig = myGamesAPIConfig.Value;
            _lookupService = lookupService;
        }

        public async Task<Amigo> GetAmigoAsync(int id)
        {
            var amigo = new Amigo();

            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo/{id}", Method.GET);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<Amigo> response = await client.ExecuteAsync<Amigo>(request);

            if (response != null)
            {
                amigo = response.Data;
            }

            return amigo;
        }

        public async Task<IList<AmigoIndexViewModel>> ListAmigoAsync([FromQuery]AmigoFilterViewModel filter)
        {
            var listAmigo = new List<AmigoIndexViewModel>();

            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo", Method.GET);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<List<Amigo>> response = await client.ExecuteAsync<List<Amigo>>(request);

            if (response != null)
            {
                var result = response.Data;

                if (!string.IsNullOrEmpty(filter.Nome))
                    result = result.Where(x => x.Nome.ToUpper().Contains(filter.Nome.ToUpper())).ToList();

                if (!string.IsNullOrEmpty(filter.Status))
                    result = result.Where(x => x.Status == filter.Status).ToList();

                listAmigo = result.Select(x =>
                new AmigoIndexViewModel
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Telefone = x.Telefone,
                    Email = x.Email,
                    JogoEmpresatado = x.JogoEmprestado,
                    Status = x.Status
                }).ToList();
            }

            return listAmigo;
        }

        public async Task<AmigoJogosViewModel> GetListJogosEmprestadosAsync(int id)
        {
            var amigo = await GetAmigoAsync(id);
            return new AmigoJogosViewModel
            {
                Nome = amigo.Nome,
                JogosEmprestados = amigo.HistoricoEmprestimo.Select(x =>
                new JogoEmprestado
                {
                    Nome = x.Nome,
                    TipoJogo = x.TipoJogo,
                    DtEmprestimo = x.DtEmprestimo
                }).ToList()
            };
        }

        public async Task<QueryResult<string>> CreateAmigoAsync(AmigoCreateViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo", Method.POST);
            request.AddJsonBody(model);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<QueryResult<string>> response = await client.ExecuteAsync<QueryResult<string>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new QueryResult<string>
                {
                    Succeeded = false,
                    Message = "Erro, problema de conexão!"
                };
            }
            else
            {
                return new QueryResult<string>
                {
                    Succeeded = response.Data.Succeeded,
                    Message = response.Data.Message
                };
            }
        }

        public async Task<AmigoEditViewModel> GetAmigoEditAsync(int id)
        {
            var amigo = await GetAmigoAsync(id);
            return new AmigoEditViewModel
            {
                Id = amigo.Id,
                Nome = amigo.Nome,
                UserName = amigo.UserName,
                Telefone = amigo.Telefone,
                Email = amigo.Email,
                Status = amigo.Status
            };
        }

        public async Task<QueryResult<string>> EditAmigoAsync(AmigoEditViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo/{id}", Method.PUT);
            request.AddParameter("id", model.Id, ParameterType.UrlSegment);
            request.AddJsonBody(model);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<QueryResult<string>> response = await client.ExecuteAsync<QueryResult<string>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new QueryResult<string>
                {
                    Succeeded = false,
                    Message = "Erro, problema de conexão!"
                };
            }
            else
            {
                return new QueryResult<string>
                {
                    Succeeded = response.Data.Succeeded,
                    Message = response.Data.Message
                };
            }
        }

        public async Task<AmigoDeleteViewModel> GetAmigoDeleteAsync(int id)
        {
            var amigo = await GetAmigoAsync(id);
            var amigoDelete = new AmigoDeleteViewModel
            {
                Id = amigo.Id,
                Nome = amigo.Nome,
                UserName = amigo.UserName,
                Telefone = amigo.Telefone,
                Email = amigo.Email,
                Status = amigo.Status
            };

            if (amigo.HistoricoEmprestimo != null)
            {
                amigoDelete.JogosEmprestados = amigo.HistoricoEmprestimo.Where(x => x.Devolvido == "Não").Select(x =>
                new JogoEmprestado
                {
                    Nome = x.Nome,
                    TipoJogo = x.TipoJogo,
                    DtEmprestimo = x.DtEmprestimo
                }).ToList();
            }

            return amigoDelete;
        }

        public async Task<QueryResult<string>> DeleteAmigoAsync(AmigoDeleteViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo/{id}", Method.DELETE);
            request.AddParameter("id", model.Id, ParameterType.UrlSegment);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<QueryResult<string>> response = await client.ExecuteAsync<QueryResult<string>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new QueryResult<string>
                {
                    Succeeded = false,
                    Message = "Erro, problema de conexão!"
                };
            }
            else
            {
                return new QueryResult<string>
                {
                    Succeeded = response.Data.Succeeded,
                    Message = response.Data.Message
                };
            }
        }
    }
}
