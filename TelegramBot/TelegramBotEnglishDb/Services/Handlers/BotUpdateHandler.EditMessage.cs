using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBotEnglishDb.Services
{
    public partial class BotUpdateHandler
    {
        private async Task HandleEditMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(message);
            var from = message.From;
            _logger.LogInformation("Received edited message from {from.Firstname}: {message.Text}", from?.FirstName, message.Text);
        }
    }
}