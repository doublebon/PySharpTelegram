using Telegram.Bot.Types.Enums;

namespace PySharpTelegram.Core.Attributes;

public abstract class MessageAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FilterByTypeAttribute : Attribute
    {
        // Свойство для хранения типа сообщения
        public MessageType[] Type { get; }

        // Конструктор для инициализации типа сообщения
        public FilterByTypeAttribute(params MessageType[] type)
        {
            Type = type;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        // Свойство для хранения списка команд
        public string[] Commands { get; }

        // Конструктор для инициализации списка команд
        public CommandAttribute(params string[] commands)
        {
            Commands = commands;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class AnyAttribute : Attribute
    {
    }
    
}