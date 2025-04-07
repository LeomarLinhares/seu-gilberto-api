using Microsoft.EntityFrameworkCore;
using PainelGilberto.Data;
using PainelGilberto.Interfaces;
using PainelGilberto.Models;

namespace PainelGilberto.Repository
{
    public class SeasonRepository : GenericRepository<Season>, ISeasonRepository
    {
        public SeasonRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Season> GetSeasonByYear(int year)
        {
            return await _context.Seasons.Where(s => s.Year == year).FirstOrDefaultAsync();
        }
    }
}
