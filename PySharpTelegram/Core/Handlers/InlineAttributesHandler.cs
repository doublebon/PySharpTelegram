using System.Reflection;
using Microsoft.Extensions.Logging;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Services.Abstract;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PySharpTelegram.Core.Handlers;

public class InlineAttributesHandler(
    ChatClassesConnector connector, 
    ILogger<InlineAttributesHandler> logger)
{
    private readonly Type[] _attrTypes =
    [
        typeof(InlineFilter.AnyAttribute)
    ];
    
    public async Task InvokeByInlineType(ITelegramBotClient botClient, InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        logger.LogInformation("Got inline message: {msg}",Newtonsoft.Json.JsonConvert.SerializeObject(inlineQuery));
        foreach (var method in connector.ChatMethods)
        {
            var methodCustomAttribute = method.GetCustomAttributes().First(attr => _attrTypes.Contains(attr.GetType()));
            switch (methodCustomAttribute)
            {
                case InlineFilter.AnyAttribute:
                    await (Task) method.Invoke(null, [botClient, inlineQuery, inlineQuery.From, cancellationToken])!;
                    return;
            }
        }
    }
}