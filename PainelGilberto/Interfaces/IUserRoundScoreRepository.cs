using PainelGilberto.Models;

namespace PainelGilberto.Interfaces
{
    public interface IUserRoundScoreRepository : IGenericRepository<UserRoundScore>
    {
        Task<UserRoundScore> GetUserRoundScoreByUserIdAndRodadaAsync(int userId, int roundId);
        Task<List<UserRoundScore>> GetbyRoundId(int roundId);
    }
}
