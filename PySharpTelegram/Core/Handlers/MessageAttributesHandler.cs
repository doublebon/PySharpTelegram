using System.Reflection;
using System.Text.RegularExpressions;
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
    ChatClassesConnector connector, 
    IChatAccessGroup? accessGroup,
    ILogger<InlineAttributesHandler> logger)
{
    private readonly Type[] _attrTypes = [
        typeof(MessageFilter.ContentTypeAttribute),
        typeof(MessageFilter.AnyAttribute),
        typeof(MessageFilter.TextAttribute),
        typeof(MessageFilter.ReplyOnTextAttribute)
    ];
    
    private readonly Type[] _restrictionsAttrTypes =
    [
        typeof(Restrictions.AccessGroups)
    ];
    
    public async Task InvokeByMessageType(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Got message: {msg}",Newtonsoft.Json.JsonConvert.SerializeObject(message));
        foreach (var method in connector.ChatMethods)
        {
            if (! await UserHasAccess(method, message.From!))
            {
                logger.LogInformation("User: {user} does not have access to perform this operation.", message.From!.Username);
                continue;
            }

            var isAnyProcessed = await MultipleFilterProcessing(botClient, method, message, cancellationToken);
            if (isAnyProcessed)
            {
                break;
            }
        }
    }

    private async Task<bool> MultipleFilterProcessing(ITelegramBotClient botClient, MethodInfo method, Message message, CancellationToken cancellationToken)
    {
        var methodCustomAttribute = method.GetCustomAttributes().Where(attr => _attrTypes.Contains(attr.GetType())).ToList();
        if(methodCustomAttribute.Count == 0) return false;

        var isAnyComplete = false;
        foreach (var attribute in methodCustomAttribute)
        {
            switch (attribute)
            {
                case MessageFilter.TextAttribute textFilter
                    when IsTextSuitable(textFilter, message.Text):
                    await (Task) method.Invoke(null, [botClient, message, message.From!, cancellationToken])!;
                    isAnyComplete = true;
                    continue;
                case MessageFilter.ReplyOnTextAttribute textFilter
                    when IsTextSuitable(textFilter, message.ReplyToMessage?.Text):
                    await (Task) method.Invoke(null, [botClient, message, message.From!, cancellationToken])!;
                    isAnyComplete = true;
                    continue;
                case MessageFilter.ContentTypeAttribute attr 
                    when attr.Type.Contains(message.Type):
                    await (Task) method.Invoke(null, [botClient, message, message.From!,  cancellationToken])!; 
                    isAnyComplete = true;
                    continue;
                case MessageFilter.AnyAttribute: 
                    await (Task) method.Invoke(null, [botClient, message, message.From!,  cancellationToken])!; 
                    isAnyComplete = true;
                    continue;
            }
        }

        return isAnyComplete;
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
        CompareType.Equals   when textFilter.SearchType is SearchType.AllOf && message is not null => message.Split(' ').SequenceEqual(textFilter.Text.SelectMany(text => text.Split(' '))),
        CompareType.Equals   when textFilter.SearchType is SearchType.AnyOf && message is not null => message.Split(' ').Intersect(textFilter.Text.SelectMany(text => text.Split(' '))).Any(),
        CompareType.Contains when textFilter.SearchType is SearchType.AllOf && message is not null => message.Split(' ').All(word => textFilter.Text.SelectMany(text => text.Split(' ')).Any(word.Contains)),
        CompareType.Contains when textFilter.SearchType is SearchType.AnyOf && message is not null => message.Split(' ').Any(word => textFilter.Text.SelectMany(text => text.Split(' ')).Any(word.Contains)),
        CompareType.Regexp   when textFilter.SearchType is SearchType.AllOf && message is not null => message.Split(' ').All(word => textFilter.Text.SelectMany(text => text.Split(' ')).Any(regex => Regex.IsMatch(word, regex))),
        CompareType.Regexp   when textFilter.SearchType is SearchType.AnyOf && message is not null => message.Split(' ').Any(word => textFilter.Text.SelectMany(text => text.Split(' ')).Any(regex => Regex.IsMatch(word, regex))),
        _ => false
    };
}