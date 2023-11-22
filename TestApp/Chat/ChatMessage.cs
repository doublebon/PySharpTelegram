using PySharpTelegram.Core.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestApp.Chat;

public class ChatMessage
{
    [Restrictions.AccessGroups("admins")]
    [MessageFilter.ByCommand("/test_access")]
    public static async Task ProcessTextRestrictions(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got access!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ByType(MessageType.Text)]
    public static async Task ProcessText(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You was send a text: {message.Text} and your id is: {user.Id}",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ByType(MessageType.Audio)]
    public static async Task ProcessAudio(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You was send a audio and your id is: {user.Id}",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ByType(MessageType.Photo)]
    public static async Task ProcessPhoto(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendPhotoAsync(
            chatId: message.Chat,
            replyToMessageId: message.MessageId,
            caption: $"You was send a photo: {message.Text} and your id is: {user.Id}",
            photo: InputFile.FromFileId(message.Photo.First().FileId),
            cancellationToken: cancellationToken);
    }
    
    [MessageFilter.Any]
    public static async Task ProcessTextAny(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"Got any filter",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ByCommand("/help")]
    public static async Task ProcessCommand(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You was send a command: {message.Text} and your id is: {user.Id}",
            cancellationToken: cancellationToken
        );
    }
}