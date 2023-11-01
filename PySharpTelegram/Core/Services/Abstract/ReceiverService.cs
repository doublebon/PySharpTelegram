using Microsoft.Extensions.Logging;
using PySharpTelegram.Core.Handlers;
using Telegram.Bot;

namespace PySharpTelegram.Core.Services.Abstract;

public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<UpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}