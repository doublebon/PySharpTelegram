using PySharpTelegram.Core.Attributes.enums;
using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Attributes;

public abstract class MessageFilter
{
    public interface ITextType
    {
        CompareType CompareType { get; }
        IEnumerable<string> Text { get; }
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ContentTypeAttribute(params MessageType[] type) : Attribute
    {
        public IEnumerable<MessageType> Type { get; } = type;
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute;
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TextAttribute(CompareType compareType, params string[] text)
        : Attribute, ITextType
    {
        public CompareType CompareType { get; } = compareType;
        public IEnumerable<string> Text { get; } = text;
        
        public TextAttribute(params string[] text) : this(CompareType.Equals, text){}
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReplyOnTextAttribute(CompareType compareType, params string[] text)
        : Attribute, ITextType
    {
        public CompareType CompareType { get; } = compareType;
        public IEnumerable<string> Text { get; } = text;
        
        public ReplyOnTextAttribute(params string[] text) : this(CompareType.Equals, text){}
    }
}