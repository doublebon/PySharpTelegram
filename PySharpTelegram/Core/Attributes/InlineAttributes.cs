namespace PySharpTelegram.Core.Attributes;

public class InlineAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute
    {
    }
}