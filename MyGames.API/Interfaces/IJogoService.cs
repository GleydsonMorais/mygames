using Microsoft.AspNetCore.Mvc;
using MyGames.API.Models.Jogo;
using MyGames.Data.Helpers;
using MyGames.Object.Jogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGames.API.Interfaces
{
    public interface IJogoService
    {
        Task<IList<JogoResult>> GetListJogoAsync();
        Task<JogoResult> GetJogoAsync(int id);
        Task<QueryResult<string>> CreateJogoAsync(JogoCreate model);
        Task<QueryResult<string>> UpdateJogoAsync(int id, string nome);
        Task<QueryResult<string>> DevolverJogoAsync(int id);
        Task<QueryResult<string>> EmprestarJogoAsync(int id, int amigoId);
        Task<QueryResult<string>> DeleteJogoAsync(int id);
    }
}
