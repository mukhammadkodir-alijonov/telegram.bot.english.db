using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TelegramBotEnglishDb.Models;
using TelegramBotEnglishDb.DbContexts;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TelegramController : ControllerBase
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly UserDbContext _dbContext;

    public TelegramController(ITelegramBotClient telegramBotClient, UserDbContext dbContext)
    {
        _telegramBotClient = telegramBotClient;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        if (update?.Message?.Text == "/start" && update.Message.Chat.Type == ChatType.Private)
        {
            var telegramUserId = update.Message.From?.Id;
            var firstName = update.Message.From?.FirstName;
            var lastName = update.Message.From?.LastName;

            var existingUser = await _dbContext.telegramUsers.FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);

            if (existingUser == null)
            {
                var newUser = new TelegramUser
                {
                    TelegramUserId = (long)telegramUserId!,
                    FirstName = firstName!,
                    LastName = lastName!,
                    // Set other properties as needed
                };

                _dbContext.telegramUsers.Add(newUser);
                await _dbContext.SaveChangesAsync();
            }
        }

        return Ok();
    }
}
