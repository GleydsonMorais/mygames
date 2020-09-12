using Microsoft.AspNetCore.Mvc;
using MyGames.Data.Helpers;
using MyGames.Web.Models.Jogo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGames.Web.Interfaces
{
    public interface IJogoService
    {
        Task<IList<JogoIndexViewModel>> ListJogoAsync([FromQuery]JogoFilterViewModel filter);
        Task<JogoHistoricoViewModel> GetJogoHistoricoAsync(int id);
        Task<QueryResult<JogoCreateViewModel>> CreateJogoAsync(JogoCreateViewModel model);
        Task<JogoEditViewModel> GetJogoEditAsync(int id);
        Task<QueryResult<JogoEditViewModel>> EditJogoAsync(JogoEditViewModel model);
        Task<QueryResult<string>> DevolverJogoAsync(int id);
    }
}
