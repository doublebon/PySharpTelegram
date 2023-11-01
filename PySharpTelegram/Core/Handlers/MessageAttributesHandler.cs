using System.Reflection;
using Microsoft.Extensions.Logging;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Services.Abstract;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Handlers;

public class MessageAttributesHandler
{
    private readonly Type[] _attrTypes = {
        typeof(MessageAttributes.CommandAttribute), 
        typeof(MessageAttributes.FilterByTypeAttribute),
        typeof(MessageAttributes.AnyAttribute)
    };

    private readonly ILogger<InlineAttributesHandler> _logger;
    private readonly MethodInfo[] _methods;

    public MessageAttributesHandler(AbstractExternalConnector connector, ILogger<InlineAttributesHandler> logger)
    {
        _logger = logger;
        _methods = connector.FindTelegramMethods(_attrTypes);
    }

    public async Task InvokeByMessageType(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        foreach (var method in _methods)
        {
            var methodCustomAttribute = method.GetCustomAttributes().First(attr => _attrTypes.Contains(attr.GetType()));
            switch (methodCustomAttribute)
            {
                case MessageAttributes.CommandAttribute command when message is { Type: MessageType.Text} && command.Commands.Contains(message.Text): 
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                case MessageAttributes.FilterByTypeAttribute attr when attr.Type.Contains(message.Type):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!,  cancellationToken })!;
                    return;
                case MessageAttributes.AnyAttribute:
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!,  cancellationToken })!;
                    return;
            }
        }
    }
}