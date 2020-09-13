using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyGames.Data.Constants;
using MyGames.Data.Helpers;
using MyGames.Object.Amigo;
using MyGames.Web.Interfaces;
using MyGames.Web.Models.Amigo;
using MyGames.Web.Models.Config;
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

        public async Task<AmigoResult> GetAmigoAsync(int id)
        {
            var amigo = new AmigoResult();

            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo/{id}", Method.GET);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<AmigoResult> response = await client.ExecuteAsync<AmigoResult>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
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

            IRestResponse<List<AmigoResult>> response = await client.ExecuteAsync<List<AmigoResult>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = response.Data;

                if (!string.IsNullOrEmpty(filter.Nome))
                    result = result.Where(x => x.Nome.ToUpper().Contains(filter.Nome.ToUpper())).ToList();

                if (filter.Status.HasValue)
                    result = result.Where(x => x.Status == filter.Status).ToList();

                listAmigo = result.Select(x =>
                new AmigoIndexViewModel
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Telefone = x.Telefone,
                    Email = x.Email,
                    JogoEmpresatado = x.JogoEmprestado,
                    Status = AccountConstants.GetStatusUsuario(x.Status)
                }).ToList();
            }

            return listAmigo;
        }

        public async Task<AmigoHistoricoEmprestimoViewModel> GetHistoricoEmprestimoAsync(int id)
        {
            var amigo = await GetAmigoAsync(id);
            return new AmigoHistoricoEmprestimoViewModel
            {
                Nome = amigo.Nome,
                Historico = amigo.HistoricoEmprestimo.Select(x =>
                new HistoricoEmprestimoViewModel
                {
                    Nome = x.Nome,
                    TipoJogo = x.TipoJogo,
                    DtEmprestimo = x.DtEmprestimo.ToShortDateString(),
                    DtDevolucao = x.DtDevolucao.HasValue ? x.DtDevolucao.Value.ToShortDateString() : null
                }).ToList()
            };
        }

        public async Task<QueryResult<AmigoCreateViewModel>> CreateAmigoAsync(AmigoCreateViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo", Method.POST);
            request.AddJsonBody(model);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<string> response = await client.ExecuteAsync<string>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return new QueryResult<AmigoCreateViewModel>
                {
                    Succeeded = true,
                    Result = model,
                    Message = response.Data
                };
            }

            return new QueryResult<AmigoCreateViewModel>
            {
                Succeeded = false,
                Result = model,
                Message = response.Data
            };
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
                Status = AccountConstants.GetStatusUsuario(amigo.Status)
            };
        }

        public async Task<QueryResult<AmigoEditViewModel>> EditAmigoAsync(AmigoEditViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo/{id}", Method.PUT);
            request.AddParameter("id", model.Id, ParameterType.UrlSegment);
            request.AddJsonBody(model);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<string> response = await client.ExecuteAsync<string>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new QueryResult<AmigoEditViewModel>
                {
                    Succeeded = true,
                    Result = model,
                    Message = "Amigo editado com sucesso."
                };
            }

            return new QueryResult<AmigoEditViewModel>
            {
                Succeeded = false,
                Result = model,
                Message = response.Data
            };
        }

        public async Task<AmigoDeleteViewModel> GetAmigoDeleteAsync(int id)
        {
            var amigo = await GetAmigoAsync(id);
            return new AmigoDeleteViewModel
            {
                Id = amigo.Id,
                Nome = amigo.Nome,
                UserName = amigo.UserName,
                Telefone = amigo.Telefone,
                Email = amigo.Email,
                Status = AccountConstants.GetStatusUsuario(amigo.Status),
                Historico = amigo.HistoricoEmprestimo.Where(x => x.Devolvido == false).Select(x =>
                new HistoricoEmprestimoViewModel
                {
                    Nome = x.Nome,
                    TipoJogo = x.TipoJogo,
                    DtEmprestimo = x.DtEmprestimo.ToShortDateString(),
                    DtDevolucao = x.DtDevolucao.HasValue ? x.DtDevolucao.Value.ToShortDateString() : null
                }).ToList()
            };
        }

        public async Task<QueryResult<string>> DeleteAmigoAsync(AmigoDeleteViewModel model)
        {
            RestClient client = new RestClient(_myGamesAPIConfig.URL);
            RestRequest request = new RestRequest("api/amigo/{id}", Method.DELETE);
            request.AddParameter("id", model.Id, ParameterType.UrlSegment);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            IRestResponse<string> response = await client.ExecuteAsync<string>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new QueryResult<string>
                {
                    Succeeded = true,
                    Message = "Amigo deletado com sucesso."
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
