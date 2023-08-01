using Microsoft.EntityFrameworkCore;
using TelegramBotEnglishDb.Entity;

namespace TelegramBotEnglishDb.DbContexts
{
    public class UserDbContext : DbContext
    {
        public virtual DbSet<TelegramUser> telegramUsers { get; set; } = default!;
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramUser>()
            .HasKey(u => u.UserId); // Replace 'Id' with the actual property name that should be the primary key
        }
    }
}
