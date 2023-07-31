namespace TelegramBotEnglishDb.Models
{
    public class TelegramUser
    {
        public int Id { get; set; }
        public long TelegramUserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
