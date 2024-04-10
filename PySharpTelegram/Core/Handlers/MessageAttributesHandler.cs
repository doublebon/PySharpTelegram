using System.Reflection;
using Microsoft.Extensions.Logging;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Attributes.enums;
using PySharpTelegram.Core.Services.Abstract;
using PySharpTelegram.Core.Services.AccessGroups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Handlers;

public class MessageAttributesHandler(
    AbstractExternalConnector connector, 
    IAccessGroup? accessGroup,
    ILogger<InlineAttributesHandler> logger)
{
    private readonly Type[] _attrTypes = [
        typeof(MessageFilter.ByCommandAttribute), 
        typeof(MessageFilter.ContentTypeAttribute),
        typeof(MessageFilter.AnyAttribute),
        typeof(MessageFilter.TextAttribute),
        typeof(MessageFilter.ReplyOnTextAttribute)
    ];
    
    private readonly Type[] _restrictionsAttrTypes =
    [
        typeof(Restrictions.AccessGroups)
    ];
    
    private IEnumerable<MethodInfo> ChatMethods => connector.FindTelegramMethods(_attrTypes);

    public async Task InvokeByMessageType(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Got message: {msg}",Newtonsoft.Json.JsonConvert.SerializeObject(message));
        foreach (var method in ChatMethods)
        {
            if (! await UserHasAccess(method, message.From!))
            {
                logger.LogInformation("User: {user} does not have access to perform this operation.", message.From!.Username);
                continue;
            }
            
            var methodCustomAttribute = method.GetCustomAttributes().FirstOrDefault(attr => _attrTypes.Contains(attr.GetType()));
            if(methodCustomAttribute == null) return;
            
            switch (methodCustomAttribute)
            {
                case MessageFilter.TextAttribute textFilter
                    when IsTextSuitable(textFilter, message.Text):
                    await (Task) method.Invoke(null, [botClient, message, message.From!, cancellationToken])!;
                    return;
                case MessageFilter.ReplyOnTextAttribute textFilter
                    when IsTextSuitable(textFilter, message.ReplyToMessage?.Text ?? ""):
                    await (Task) method.Invoke(null, [botClient, message, message.From!, cancellationToken])!;
                    return;
                case MessageFilter.ContentTypeAttribute attr 
                    when attr.Type.Contains(message.Type):
                    await (Task) method.Invoke(null, [botClient, message, message.From!,  cancellationToken])!; 
                    return;
                case MessageFilter.AnyAttribute: 
                    await (Task) method.Invoke(null, [botClient, message, message.From!,  cancellationToken])!; 
                    return;
            }
        }
    }

    private async Task<bool> UserHasAccess(MemberInfo method, User user)
    {
        if (accessGroup == null)
        {
            return true;
        }
        
        var restrictionsAttribute = method.GetCustomAttributes().FirstOrDefault(attr => _restrictionsAttrTypes.Contains(attr.GetType()));
        return restrictionsAttribute switch
        {
            Restrictions.AccessGroups accessGroupName when (await accessGroup.GetGroupMembersAsync(accessGroupName.AccessGroupName)).Any(privileged => privileged.Username!.Contains(user.Username!, StringComparison.OrdinalIgnoreCase)) => true,
            null => true,
            _ => false
        };
    }

    private static bool IsTextSuitable(MessageFilter.ITextType textFilter, string? message) => textFilter.CompareType switch
    {
        CompareType.Equals   when textFilter.SearchType is SearchType.AllOf && message is not null => message.Split(' ').SequenceEqual(textFilter.Text),
        CompareType.Equals   when textFilter.SearchType is SearchType.AnyOf && message is not null => message.Split(' ').Intersect(textFilter.Text).Any(),
        CompareType.Contains when textFilter.SearchType is SearchType.AnyOf && message is not null => message.Split(' ').Any(word => textFilter.Text.Any(word.Contains)),
        CompareType.Contains when textFilter.SearchType is SearchType.AllOf && message is not null => message.Split(' ').All(word => textFilter.Text.Any(word.Contains)),
        _ => throw new ArgumentOutOfRangeException("Unknown compare type")
    };
}