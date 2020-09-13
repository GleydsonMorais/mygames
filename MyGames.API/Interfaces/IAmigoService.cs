using Microsoft.AspNetCore.Mvc;
using MyGames.API.Models.Amigo;
using MyGames.Data.Helpers;
using MyGames.Object.Amigo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGames.API.Interfaces
{
    public interface IAmigoService
    {
        Task<IList<AmigoResult>> GetListAmigoAsync();
        Task<AmigoResult> GetAmigoAsync(int id);
        Task<QueryResult<string>> CreateAmigoAsync([FromBody]AmigoCreate model);
        Task<QueryResult<string>> UpdateAmigoAsync(int id, [FromBody]AmigoEdit model);
        Task<QueryResult<string>> DeleteAmigoAsync(int id);
    }
}
