namespace PySharpTelegram.Core.Attributes;

public abstract class InlineFilter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute;
}