using Microsoft.EntityFrameworkCore;
using MyGames.Data.Contexts;
using MyGames.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGames.Data.Repositories
{
    public interface IAmigoRepository
    {
        Task<Pessoa> GetAmigoAsync(int id);
    }

    public class AmigoRepository : IAmigoRepository
    {
        public readonly MyGamesDbContext _dataContext;

        public AmigoRepository(MyGamesDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Pessoa> GetAmigoAsync(int id) => await _dataContext.Pessoas
            .Include(x => x.TipoPessoa)
            .Include(x => x.Login)
            .Include(x => x.JogosEmprestados).ThenInclude(x => x.Jogo).ThenInclude(x => x.TipoJogo)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}
