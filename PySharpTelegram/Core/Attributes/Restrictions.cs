namespace PySharpTelegram.Core.Attributes;

public abstract class Restrictions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AccessGroups(params string[] groupName) : Attribute
    {
        public string[] AccessGroupName { get; } = groupName;
    }
}