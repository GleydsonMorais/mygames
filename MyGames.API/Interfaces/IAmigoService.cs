using Microsoft.AspNetCore.Mvc;
using MyGames.API.Models.Amigo;
using MyGames.Data.Helpers;
using MyGames.Object.Amigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Interfaces
{
    public interface IAmigoService
    {
        Task<Amigo> GetAmigoAsync(int id);
        Task<IList<Amigo>> GetListAmigoAsync();
        Task<QueryResult<string>> CreateAmigoAsync([FromBody]AmigoCreate model);
        Task<QueryResult<string>> EditAmigoAsync(int id, [FromBody]AmigoEdit model);
        Task<QueryResult<string>> DeleteAmigoAsync(int id);
    }
}
