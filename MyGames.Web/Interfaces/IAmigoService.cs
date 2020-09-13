using Microsoft.AspNetCore.Mvc;
using MyGames.Data.Helpers;
using MyGames.Object.Amigo;
using MyGames.Web.Models.Amigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.Web.Interfaces
{
    public interface IAmigoService
    {
        Task<AmigoResult> GetAmigoAsync(int id);
        Task<IList<AmigoIndexViewModel>> ListAmigoAsync([FromQuery]AmigoFilterViewModel filter);
        Task<AmigoHistoricoEmprestimoViewModel> GetHistoricoEmprestimoAsync(int id);
        Task<QueryResult<AmigoCreateViewModel>> CreateAmigoAsync(AmigoCreateViewModel model);
        Task<AmigoEditViewModel> GetAmigoEditAsync(int id);
        Task<QueryResult<AmigoEditViewModel>> EditAmigoAsync(AmigoEditViewModel model);
        Task<AmigoDeleteViewModel> GetAmigoDeleteAsync(int id);
        Task<QueryResult<string>> DeleteAmigoAsync(AmigoDeleteViewModel model);
    }
}
