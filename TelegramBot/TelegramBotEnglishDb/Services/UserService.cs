using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotEnglishDb.DbContexts;
using TelegramBotEnglishDb.Entity;
using TelegramBotEnglishDb.Interfaces.UserInterfaces;

namespace TelegramBotEnglishDb.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _userDbContext;

        public UserService(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        /*public async Task<TelegramUser?> GetUserAsync(long? userId)
        {
            ArgumentNullException.ThrowIfNull(userId);

            return await _userDbContext.telegramUsers.FindAsync(userId);
        }*/
        public async Task AddUserAsync(Update update)
        {
            if (update.Message != null && update.Message.Text == "/start")
            {
                var telegramUserId = update.Message.From!.Id;
                var firstName = update.Message.From.FirstName;
                var lastName = update.Message.From.LastName;
                var chatId = update.Message.Chat.Id;
                var isBot = update.Message.From.IsBot;
                var userName = update.Message.From.Username;
                var createdAt = update.Message.Date;
                var lastInteractionAt = update.Message.Date;

                var existingUser = await _userDbContext.telegramUsers.FirstOrDefaultAsync(u => u.UserId == telegramUserId);

                if (existingUser == null)
                {
                    var newUser = new TelegramUser
                    {
                        UserId = telegramUserId,
                        FirstName = firstName,
                        LastName = lastName,
                        ChatId = chatId,
                        IsBot = isBot,
                        Username = userName,
                        CreatedAt = createdAt,
                        LastInteractionAt = lastInteractionAt
                        // Set other properties as needed
                    };

                    _userDbContext.telegramUsers.Add(newUser);
                    await _userDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
