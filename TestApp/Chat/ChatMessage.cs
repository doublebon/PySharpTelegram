using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Attributes.enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestApp.Chat;

public class ChatMessage
{
    //Multiple filters usage. Invoke if fits one of this filters
    [MessageFilter.Text(CompareType.Equals, text: "/aaa" )]
    [MessageFilter.Text(CompareType.Equals, text: "/bbb" )]
    public static async Task ProcessTextEqualsAny(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Equals any command!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Contains, ["alal", "akek"] )]
    public static async Task ProcessTextContainsAny(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Contains any!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Contains, ["Hello world"] )]
    public static async Task ProcessTextContainsAllOfTabText(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got Contains Tab text!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Equals, ["Hello two"] )]
    public static async Task ProcessTextEqualsAllOfTabText(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got EqualsAllOfTabText!",
            cancellationToken: cancellationToken
        );
    }
    
    [Restrictions.AccessGroups("admins")]
    [MessageFilter.Text(CompareType.Equals, text: "/test_access" )]
    public static async Task ProcessTextRestrictions(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You got access!",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.ContentType(MessageType.Audio)]
    [MessageFilter.ContentType(MessageType.Photo)]
    [MessageFilter.ContentType(MessageType.Document)]
    public static async Task ProcessContentType(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"You was send a content type and your id is: {user.Id}",
            cancellationToken: cancellationToken
        );
    }
    
    [MessageFilter.Text(CompareType.Regexp, @"^[0-9]*$")]
    public static async Task ProcessRegexpAllNumbers(ITelegramBotClient bot, Message message, User user, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(
            chatId: message.Chat,
            text: $"Got regexp all of filter",
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