using Microsoft.EntityFrameworkCore;
using MyGames.Data.Contexts;
using MyGames.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGames.Data.Repositories
{
    public interface IJogoRepository
    {
        Task<IList<Jogo>> AllAsync();
        Task<Jogo> GetJogoAsync(int id);
        Task<TipoJogo> GetTipoJogoAsync(int TipoJogoId);
        Task<Jogo> ValidateGameAsync(string Nome, int TipoJogoId);
        Task InsertJogoAsync(Jogo jogo);
        Task UpdateJogoAsync(Jogo jogo);
        Task DeleteJogoAsync(Jogo jogo);
    }

    public class JogoRepository : IJogoRepository
    {
        public readonly MyGamesDbContext _dataContext;

        public JogoRepository(MyGamesDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<Jogo>> AllAsync() => await _dataContext.Jogos
            .Include(x => x.TipoJogo)
            .Include(x => x.HistoricoEmprestimo).ThenInclude(x => x.Pessoa).ThenInclude(x => x.Login)
            .ToListAsync();

        public async Task<Jogo> GetJogoAsync(int id) => await _dataContext.Jogos
            .Include(x => x.TipoJogo)
            .Include(x => x.HistoricoEmprestimo).ThenInclude(x => x.Pessoa).ThenInclude(x => x.Login)
            .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<TipoJogo> GetTipoJogoAsync(int TipoJogoId) => await _dataContext.TipoJogos
            .SingleOrDefaultAsync(x => x.Id == TipoJogoId);

        public async Task<Jogo> ValidateGameAsync(string Nome, int TipoJogoId) => await _dataContext.Jogos
            .SingleOrDefaultAsync(x => x.Nome.ToLower() == Nome.ToLower() && x.TipoJogoId == TipoJogoId);

        public async Task InsertJogoAsync(Jogo jogo)
        {
            await _dataContext.AddAsync(jogo);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateJogoAsync(Jogo jogo)
        {
            _dataContext.Update(jogo);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteJogoAsync(Jogo jogo)
        {
            _dataContext.Remove(jogo);
            await _dataContext.SaveChangesAsync();
        }
    }
}
