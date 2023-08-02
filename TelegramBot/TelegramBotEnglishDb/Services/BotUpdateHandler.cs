using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotEnglishDb.Services
{
    public partial class BotUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<BotUpdateHandler> _logger;

        public BotUpdateHandler(ILogger<BotUpdateHandler> logger)
        {
            _logger = logger;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Error occured with Telegram Bot: {e.Message}", exception);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update.Message!, cancellationToken),
                UpdateType.EditedMessage => HandleEditMessageAsync(botClient, update.EditedMessage!, cancellationToken),
                _ => HandleUnknownUpdate(botClient, update, cancellationToken)
            };
            try
            {
                await handler;
            }
            catch (Exception e)
            {
                await HandlePollingErrorAsync(botClient, e, cancellationToken);
            }
        }

        private Task HandleUnknownUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update type {update.Type} recived", update.Type);

            return Task.CompletedTask;
        }
    }
}
