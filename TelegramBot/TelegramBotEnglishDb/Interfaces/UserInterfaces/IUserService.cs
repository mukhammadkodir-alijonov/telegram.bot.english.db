using Telegram.Bot.Types;
using TelegramBotEnglishDb.Entity;

namespace TelegramBotEnglishDb.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        public Task AddUserAsync(Update update);
    }
}
