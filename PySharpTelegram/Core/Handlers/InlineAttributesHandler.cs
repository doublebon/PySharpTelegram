using System.Reflection;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Support;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PySharpTelegram.Core.Handlers;

public static class InlineAttributesHandler
{
    private static readonly Type[] AttrTypes = {
        typeof(InlineAttributes.AnyAttribute),
    };
    
    private static readonly MethodInfo[] Methods = SupportUtils.ParseAllChatMethodsWithAttributes("chat", AttrTypes);
    
    public static async Task InvokeByInlineType(ITelegramBotClient botClient, InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        foreach (var method in Methods)
        {
            var methodCustomAttribute = method.GetCustomAttributes().First(attr => AttrTypes.Contains(attr.GetType()));
            switch (methodCustomAttribute)
            {
                case InlineAttributes.AnyAttribute:
                    await (Task) method.Invoke(null, new object[] { botClient, inlineQuery, inlineQuery.From, cancellationToken })!;
                    return;
            }
        }
    }
}