namespace PySharpTelegram.Core.Attributes;

public class Restrictions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AccessGroups : Attribute
    {
        public string[] AccessGroupName { get; }
        
        public AccessGroups(params string[] groupName)
        {
            AccessGroupName = groupName;
        }
    }
}