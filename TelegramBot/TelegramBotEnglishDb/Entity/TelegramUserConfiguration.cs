using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Bot.Types;

namespace TelegramBotEnglishDb.Entity
{
    public class TelegramUserConfiguration : IEntityTypeConfiguration<TelegramUser>
    {
        public void Configure(EntityTypeBuilder<TelegramUser> builder)
        {
            builder.HasKey(b => b.UserId);
            builder.HasIndex(b => b.ChatId).IsUnique(false);
            builder.Property(b => b.FirstName).HasMaxLength(255);
            builder.Property(b => b.LastName).HasMaxLength(255);
        }
    }
}
