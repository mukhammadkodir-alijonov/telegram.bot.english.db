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
            _logger.LogError("An unhandled exception was thrown by the bot:{e.Message} ", exception);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
                UpdateType.EditedMessage => HandleEditedMessageAsync(botClient, update.EditedMessage, cancellationToken),
                //handle other updates
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
            _logger.LogInformation("Update of type {update.Type} received.",update.Type);
            return Task.CompletedTask;
        }
    }
}
