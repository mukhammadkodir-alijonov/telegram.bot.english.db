using Telegram.Bot;
using Telegram.Bot.Polling;
namespace TelegramBotEnglishDb.Services
{
    public class BotBackgroundService : BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly TelegramBotClient _client;
        private readonly IUpdateHandler _handler;

        public BotBackgroundService(ILogger<BackgroundService> logger,
            TelegramBotClient client,
            IUpdateHandler handler)
        {
            _logger = logger;
            _client = client;
            _handler = handler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bot = await _client.GetMeAsync(stoppingToken);
            _logger.LogInformation("Bot Started successfully: {bot.Username}", bot.Username);
            _client.StartReceiving(
                _handler.HandleUpdateAsync,
                _handler.HandlePollingErrorAsync,
                new ReceiverOptions
                {
                    ThrowPendingUpdates = true
                }, stoppingToken);
        }
    }
}