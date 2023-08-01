using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBotEnglishDb.Services
{
    public partial class BotUpdateHandler
    {
        private async Task HandleEditedMessageAsync(ITelegramBotClient botClient, Message? editedMessage, CancellationToken cancellationToken)
        {
            var from = editedMessage?.From;
            ArgumentNullException.ThrowIfNull(editedMessage);
            _logger.LogInformation("EditedMessage received from: {from.Firstname}: {message.Text}", from?.FirstName, editedMessage.Text);
        }

    }
}
