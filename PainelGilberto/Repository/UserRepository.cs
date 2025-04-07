using Microsoft.EntityFrameworkCore;
using PainelGilberto.Data;
using PainelGilberto.Interfaces;
using PainelGilberto.Models;

namespace PainelGilberto.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<User> GetUserByTelegramIdAsync(long telegramId)
        {
            return await _context.Users.Where(u => u.TelegramUserId == telegramId).FirstOrDefaultAsync();
        }
    }
}
