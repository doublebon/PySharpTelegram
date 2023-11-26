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
    public class ByTextEqualsAttribute : Attribute
    {
        public string Text { get; }
        
        public ByTextEqualsAttribute(string text)
        {
            Text = text;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ByTextContainsAttribute : Attribute
    {
        public string[] Texts { get; }
        
        public ByTextContainsAttribute(params string[] texts)
        {
            Texts = texts;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ByReplyOnTextEqualsAttribute : Attribute
    {
        public string Text { get; }
        
        public ByReplyOnTextEqualsAttribute(string text)
        {
            Text = text;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ByReplyOnTextContainsAttribute : Attribute
    {
        public string[] Texts { get; }
        
        public ByReplyOnTextContainsAttribute(params string[] texts)
        {
            Texts = texts;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute
    {
    }
    
}