﻿using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotEnglishDb.Services
{
    public partial class BotUpdateHandler
    {
        private async Task HandleMessageAsync(ITelegramBotClient botClient, Message? message,CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(message);
            
            var from = message?.From;
            _logger.LogInformation("Message received from: {from.Firstname}",from?.FirstName);

            var handler = message?.Type switch
            {
                MessageType.Text => HandleTextMessageAsync(botClient, message, cancellationToken),
                _ => HandleUnknownMessageAsync(botClient, message!, cancellationToken)
            };
            await handler;
        }

        private Task HandleUnknownMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown message received type: {message.Type}", message.Type);
            return Task.CompletedTask;
        }

        private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
        {
            var from = message?.From;
            _logger.LogInformation("From message received from: {from.Firstname}, {from.Type}", from?.FirstName, message!.Type);

            await botClient.SendTextMessageAsync(
                chatId: message!.Chat.Id,
                text: "Thank You!",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
                );
        }
    }
}
