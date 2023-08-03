using Microsoft.EntityFrameworkCore;
using TelegramBotEnglishDb.Entity;

namespace TelegramBotEnglishDb.AppDbContext
{
    public class BotDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {

        }
    }
}
