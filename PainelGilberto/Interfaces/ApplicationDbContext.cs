using Microsoft.EntityFrameworkCore;
using PainelGilberto.Models;

namespace PainelGilberto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Round> Rounds { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoundScore> UserRoundScores { get; set; }
    }
}
