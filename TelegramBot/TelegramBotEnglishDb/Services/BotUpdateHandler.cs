using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotEnglishDb.AppDbContext;

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
           /* if (update.Type == UpdateType.Message && update.Message?.Type == MessageType.Text)
            {
                // Check if the user is starting the bot (you can use a specific command or condition).
                if (update.Message.Text == "/start")
                {
                    var username = update.Message.From!.Username; // Get the username of the user.

                    // Check if the user already exists in the database.
                    var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

                    if (existingUser == null)
                    {
                        // User doesn't exist, create a new user and add it to the database.
                        var newUser = new Entity.User { 
                            UserId = update.Message.From.Id,
                            ChatId = update.Message.Chat.Id,
                            IsBot = update.Message.From.IsBot,
                            FirstName = update.Message.From.FirstName,
                            LastName = update.Message.From.LastName,
                            Username = update.Message.From.Username,
                            CreatedAt = DateTimeOffset.UtcNow,
                            LastInteractionAt = DateTimeOffset.UtcNow

                        };
                        _dbContext.Users.Add(newUser);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }*/
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