using PainelGilberto.Models;

namespace PainelGilberto.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByTelegramIdAsync(long telegramId);
    }
}
