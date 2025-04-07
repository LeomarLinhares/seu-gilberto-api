using Microsoft.EntityFrameworkCore;
using PainelGilberto.Data;
using PainelGilberto.Interfaces;
using PainelGilberto.Models;

namespace PainelGilberto.Repository
{
    public class UserRoundScoreRepository : GenericRepository<UserRoundScore>, IUserRoundScoreRepository
    {
        public UserRoundScoreRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserRoundScore> GetUserRoundScoreByUserIdAndRodadaAsync(int userId, int roundId)
        {
            return await _context.UserRoundScores
                .Where(urs => urs.UserId == userId && urs.RoundId == roundId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserRoundScore>> GetbyRoundId(int roundId)
        {
            return await _context.UserRoundScores
                .Where(urs => urs.RoundId == roundId)
                .ToListAsync();
        }
    }
}
