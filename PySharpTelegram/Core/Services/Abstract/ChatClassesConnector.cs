using System.Reflection;

namespace PySharpTelegram.Core.Services.Abstract;

public class ChatClassesConnector(params Type[] chatTypeOfClasses)
{
    public IEnumerable<MethodInfo> ChatMethods { get; } = 
        chatTypeOfClasses.SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public)).ToArray();

    public static ChatClassesConnector Create(params Type[] chatClasses) => new (chatClasses);
}