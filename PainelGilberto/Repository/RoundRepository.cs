using Microsoft.EntityFrameworkCore;
using PainelGilberto.Data;
using PainelGilberto.Interfaces;
using PainelGilberto.Models;

namespace PainelGilberto.Repository
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        private readonly ApplicationDbContext _context;
        public RoundRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Round> GetRoundByNumberAndSeasonAsync(int roundNumber, int seasonId)
        {
            return await _context.Rounds.Where(r => r.RoundNumber == roundNumber && r.SeasonId == seasonId).FirstOrDefaultAsync();
        }
    }
}
