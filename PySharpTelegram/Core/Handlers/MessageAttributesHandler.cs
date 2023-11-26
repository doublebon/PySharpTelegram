using System.Reflection;
using Microsoft.Extensions.Logging;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Services.Abstract;
using PySharpTelegram.Core.Services.AccessGroups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Handlers;

public class MessageAttributesHandler
{
    private readonly Type[] _attrTypes = {
        typeof(MessageFilter.ByCommandAttribute), 
        typeof(MessageFilter.ByTypeAttribute),
        typeof(MessageFilter.ByTextEqualsAttribute),
        typeof(MessageFilter.ByTextContainsAttribute),
        typeof(MessageFilter.ByReplyOnTextEqualsAttribute),
        typeof(MessageFilter.ByReplyOnTextContainsAttribute),
        typeof(MessageFilter.AnyAttribute)
    };
    
    private readonly Type[] _restrictionsAttrTypes = {
        typeof(Restrictions.AccessGroups),
    };

    private readonly ILogger<InlineAttributesHandler> _logger;
    private readonly MethodInfo[] _methods;
    private readonly IAccessGroup? _accessGroup;
    
    public MessageAttributesHandler(AbstractExternalConnector connector, ILogger<InlineAttributesHandler> logger) : this(connector, null, logger)
    {
    }

    public MessageAttributesHandler(
        AbstractExternalConnector connector, 
        IAccessGroup? accessGroup,
        ILogger<InlineAttributesHandler> logger)
    {
        _methods = connector.FindTelegramMethods(_attrTypes);
        _accessGroup = accessGroup;
        _logger = logger;
    }

    public async Task InvokeByMessageType(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Got message: {msg}",Newtonsoft.Json.JsonConvert.SerializeObject(message));
        foreach (var method in _methods)
        {
            if (! await UserHasAccess(method, message.From!))
            {
                _logger.LogInformation("User: {user} does not have access to perform this operation.", message.From!.Username);
                continue;
            }
            
            var methodCustomAttribute = method.GetCustomAttributes().FirstOrDefault(attr => _attrTypes.Contains(attr.GetType()));
            if(methodCustomAttribute == null) return;
            
            switch (methodCustomAttribute)
            {
                case MessageFilter.ByCommandAttribute command 
                    when message is { Type: MessageType.Text, Text: { } } && message.Text.StartsWith("/") && command.Commands.Contains(message.Text): 
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                
                case MessageFilter.ByTextEqualsAttribute expTextEquals
                    when message is { Type: MessageType.Text, Text: { } } && message.Text.Equals(expTextEquals.Text, StringComparison.OrdinalIgnoreCase):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                
                case MessageFilter.ByTextContainsAttribute expTextContains
                    when message is { Type: MessageType.Text, Text: { } } && ContainsPhrase(message.Text, expTextContains.Texts):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                
                case MessageFilter.ByReplyOnTextEqualsAttribute expReplyOnTextEquals
                    when message.ReplyToMessage is { Type: MessageType.Text, Text: { } } && expReplyOnTextEquals.Text.Equals(message.ReplyToMessage.Text, StringComparison.OrdinalIgnoreCase):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                
                case MessageFilter.ByReplyOnTextContainsAttribute expReplyOnTextContains
                    when message.ReplyToMessage is { Type: MessageType.Text, Text: { } } && ContainsPhrase(message.ReplyToMessage.Text, expReplyOnTextContains.Texts):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!, cancellationToken })!;
                    return;
                
                case MessageFilter.ByTypeAttribute attr 
                    when attr.Type.Contains(message.Type):
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!,  cancellationToken })!;
                    return;
                
                case MessageFilter.AnyAttribute:
                    await (Task) method.Invoke(null, new object[] { botClient, message, message.From!,  cancellationToken })!;
                    return;
            }
        }
    }

    private async Task<bool> UserHasAccess(MemberInfo method, User user)
    {
        if (_accessGroup == null)
        {
            return true;
        }
        
        var restrictionsAttribute = method.GetCustomAttributes().FirstOrDefault(attr => _restrictionsAttrTypes.Contains(attr.GetType()));
        return restrictionsAttribute switch
        {
            Restrictions.AccessGroups accessGroupName when (await _accessGroup.GetGroupMembersAsync(accessGroupName.AccessGroupName)).Any(privileged => privileged.Username!.Contains(user.Username!, StringComparison.OrdinalIgnoreCase)) => true,
            null => true,
            _ => false
        };
    }
    
    bool ContainsPhrase(string text, IEnumerable<string> phrases)
    {
        string[] textWords = System.Text.RegularExpressions.Regex.Split(text.ToLower(), @"\W+");

        return phrases.Any(phrase =>
        {
            string[] phraseWords = System.Text.RegularExpressions.Regex.Split(phrase.ToLower(), @"\W+");
            int phraseStartIndex = Array.IndexOf(textWords, phraseWords[0]);

            if (phraseStartIndex < 0 || phraseStartIndex + phraseWords.Length > textWords.Length)
            {
                return false;
            }

            for (var i = 0; i < phraseWords.Length; i++)
            {
                if (!textWords[phraseStartIndex + i].Equals(phraseWords[i]))
                {
                    return false;
                }
            }

            return true;
        });
    }
}