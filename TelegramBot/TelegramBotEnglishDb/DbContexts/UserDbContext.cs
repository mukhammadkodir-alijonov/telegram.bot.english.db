using Microsoft.EntityFrameworkCore;
using TelegramBotEnglishDb.Models;

namespace TelegramBotEnglishDb.DbContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<TelegramUser> telegramUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
