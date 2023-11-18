namespace PySharpTelegram.Core.Attributes;

public class InlineFilter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute
    {
    }
}