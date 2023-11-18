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
        typeof(MessageFilter.ByCommandAttribute), 
        typeof(MessageFilter.ByTypeAttribute),
        typeof(MessageFilter.AnyAttribute)
    };
    
    private readonly Type[] _restrictionsAttrTypes = {
        typeof(Restrictions.AccessForUsersAttribute),
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
        _logger.LogInformation("Got message: {msg}",Newtonsoft.Json.JsonConvert.SerializeObject(message));
        foreach (var method in _methods)
        {
            if (!UserHasAccess(method, message.From!))
            {
                _logger.LogInformation("User: {user} does not have access to perform this operation.", message.From!.Username);
                continue;
            }
            
            var methodCustomAttribute = method.GetCustomAttributes().FirstOrDefault(attr => _attrTypes.Contains(attr.GetType()));
            if(methodCustomAttribute == null) return;
            
            switch (methodCustomAttribute)
            {
                case MessageFilter.ByCommandAttribute command 
                    when message is { Type: MessageType.Text, Text: { } } && command.Commands.Contains(message.Text): 
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                
                case MessageFilter.ByTypeAttribute attr when attr.Type.Contains(message.Type):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!,  cancellationToken })!;
                    return;
                
                case MessageFilter.AnyAttribute:
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!,  cancellationToken })!;
                    return;
            }
        }
    }

    private bool UserHasAccess(MemberInfo method, User user)
    {
        var restrictionsAttribute = method.GetCustomAttributes().FirstOrDefault(attr => _restrictionsAttrTypes.Contains(attr.GetType()));
        
        return restrictionsAttribute switch
        {
            Restrictions.AccessForUsersAttribute access when access.AccessByUserName.Any(privileged => privileged.Contains(user.Username!)) => true,
            null => true,
            _ => false
        };
    }
}