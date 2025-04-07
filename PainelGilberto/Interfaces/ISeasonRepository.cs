using PainelGilberto.Models;

namespace PainelGilberto.Interfaces
{
    public interface ISeasonRepository : IGenericRepository<Season>
    {
        public Task<Season> GetSeasonByYear(int year);
    }
}
