using Microsoft.EntityFrameworkCore;
using MyGames.Data.Constants;
using MyGames.Data.Contexts;
using MyGames.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGames.Data.Repositories
{
    public interface IAmigoRepository
    {
        Task<IList<Pessoa>> AllAsync();
        Task<Pessoa> GetAmigoAsync(int id);
        Task<Pessoa> ValidateEmialAsync(string email);
        Task UpdateAmigoAsync(Pessoa amigo);
        Task DeleteAmigoAsync(Pessoa amigo);
    }

    public class AmigoRepository : IAmigoRepository
    {
        public readonly MyGamesDbContext _dataContext;

        public AmigoRepository(MyGamesDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<Pessoa>> AllAsync() => await _dataContext.Pessoas
            .Include(x => x.TipoPessoa)
            .Include(x => x.Login)
            .Include(x => x.HistoricoEmprestimos)
            .Where(x => x.TipoPessoaId == AccountConstants.AmigoDB)
            .ToListAsync();

        public async Task<Pessoa> GetAmigoAsync(int id) => await _dataContext.Pessoas
            .Include(x => x.TipoPessoa)
            .Include(x => x.Login)
            .Include(x => x.HistoricoEmprestimos).ThenInclude(x => x.Jogo).ThenInclude(x => x.TipoJogo)
            .SingleOrDefaultAsync(x => x.Id == id && x.TipoPessoaId == AccountConstants.AmigoDB);

        public async Task<Pessoa> ValidateEmialAsync(string email) => await _dataContext.Pessoas
            .Include(x => x.Login)
            .SingleOrDefaultAsync(x => x.Login.Email.ToLower() == email.ToLower());

        public async Task UpdateAmigoAsync(Pessoa amigo)
        {
            _dataContext.Update(amigo);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAmigoAsync(Pessoa amigo)
        {
            _dataContext.Remove(amigo);
            _dataContext.Remove(amigo.Login);
            await _dataContext.SaveChangesAsync();
        }
    }
}
