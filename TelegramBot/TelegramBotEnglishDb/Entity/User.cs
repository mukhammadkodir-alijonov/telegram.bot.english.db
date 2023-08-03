namespace TelegramBotEnglishDb.Entity
{
    public class User
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public bool IsBot { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastInteractionAt { set; get; }
    }
}
