using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotEnglishDb.Entity;
using TelegramBotEnglishDb.Interfaces.UserInterfaces;

namespace TelegramBotEnglishDb.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddNewUserDbAsync([FromBody] Update update)
        {
            await _userService.AddUserAsync(update);
            return Ok();
        }
    }
}
