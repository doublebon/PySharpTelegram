namespace PySharpTelegram.Core.Attributes;

public class Restrictions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AccessForUsersAttribute : Attribute
    {
        public string[] AccessByUserName { get; }
        
        public AccessForUsersAttribute(params string[] accessByUserName)
        {
            AccessByUserName = accessByUserName;
        }
    }
}