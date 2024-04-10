using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Attributes.enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestApp.Chat;

public class ChatMessage
{
    [MessageFilter.Text(CompareType.Equals, SearchType.AllOf, text: "/aaa" )]
    [MessageFilter.Text(CompareType.Equals, SearchType.AllOf, text: "/bbb" )]
    public static async Task ProcessTextEqualsAllOf(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Equals AllOf!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Equals, SearchType.AnyOf, text: ["lal", "kek"] )]
    public static async Task ProcessTextEqualsAnyOf(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Equals AnyOf!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Contains, SearchType.AllOf, ["alal", "akek"] )]
    public static async Task ProcessTextContainsAllOf(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Contains AllOf!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Contains, SearchType.AnyOf, ["blal", "bkek"] )]
    public static async Task ProcessTextContainsAnyOf(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Contains AnyOf!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ReplyOnText(CompareType.Contains, SearchType.AnyOf, ["blal", "bkek"] )]
    public static async Task ProcessReplyOnTextContainsAnyOf(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Contains AnyOf!",
            cancellationToken: cancellationToken
        );
    }
    
    [Restrictions.AccessGroups("admins")]
    [MessageFilter.Text(CompareType.Equals, SearchType.AllOf, text: "/test_access" )]
    public static async Task ProcessTextRestrictions(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got access!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ContentType(MessageType.Audio)]
    public static async Task ProcessAudio(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You was send a audio and your id is: {user.Id}",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Any]
    public static async Task ProcessAny(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"Got any filter",
            cancellationToken: cancellationToken
        );
    }
}