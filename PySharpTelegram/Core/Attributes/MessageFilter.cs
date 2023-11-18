using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Attributes;

public abstract class MessageFilter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ByTypeAttribute : Attribute
    {
        public MessageType[] Type { get; }
        
        public ByTypeAttribute(params MessageType[] type)
        {
            Type = type;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ByCommandAttribute : Attribute
    {
        public string[] Commands { get; }
        
        public ByCommandAttribute(params string[] commands)
        {
            Commands = commands;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute
    {
    }
    
}