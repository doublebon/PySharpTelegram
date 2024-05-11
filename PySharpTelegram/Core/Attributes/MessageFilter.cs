using PySharpTelegram.Core.Attributes.enums;
using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Attributes;

public abstract class MessageFilter
{
    public interface ITextType
    {
        CompareType CompareType { get; }
        SearchType SearchType { get; }
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
    public class TextAttribute(CompareType compareType, SearchType searchType, params string[] text)
        : Attribute, ITextType
    {
        public CompareType CompareType { get; } = compareType;
        public SearchType SearchType { get; } = searchType;
        public IEnumerable<string> Text { get; } = text;
        
        public TextAttribute(params string[] text) : this(CompareType.Equals, SearchType.AnyOf, text){}
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReplyOnTextAttribute(CompareType compareType, SearchType searchType, params string[] text)
        : Attribute, ITextType
    {
        public CompareType CompareType { get; } = compareType;
        public SearchType SearchType { get; } = searchType;
        public IEnumerable<string> Text { get; } = text;
        
        public ReplyOnTextAttribute(params string[] text) : this(CompareType.Equals, SearchType.AnyOf, text){}
    }
}