using System.Reflection;
using Microsoft.Extensions.Logging;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Services.Abstract;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PySharpTelegram.Core.Handlers;

public class InlineAttributesHandler
{
    private readonly Type[] _attrTypes = {
        typeof(InlineAttributes.AnyAttribute),
    };
    
    private readonly ILogger<InlineAttributesHandler> _logger;
    private readonly MethodInfo[] _methods;

    public InlineAttributesHandler(AbstractExternalConnector connector, ILogger<InlineAttributesHandler> logger)
    {
        _logger = logger;
        _methods = connector.FindTelegramMethods(_attrTypes);
    }
    
    public async Task InvokeByInlineType(ITelegramBotClient botClient, InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("aaaaa");
        foreach (var method in _methods)
        {
            var methodCustomAttribute = method.GetCustomAttributes().First(attr => _attrTypes.Contains(attr.GetType()));
            switch (methodCustomAttribute)
            {
                case InlineAttributes.AnyAttribute:
                    await (Task) method.Invoke(null, new object[] { botClient, inlineQuery, inlineQuery.From, cancellationToken })!;
                    return;
            }
        }
    }
}