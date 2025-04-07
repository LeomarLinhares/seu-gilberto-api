using PainelGilberto.Models;

namespace PainelGilberto.Interfaces
{
    public interface IRoundRepository : IGenericRepository<Round>
    {
        Task<Round> GetRoundByNumberAndSeasonAsync(int roundNumber, int seasonId);
    }

}
