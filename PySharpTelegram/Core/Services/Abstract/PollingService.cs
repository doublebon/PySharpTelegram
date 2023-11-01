using Microsoft.Extensions.Logging;

namespace PySharpTelegram.Core.Services.Abstract;

public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
    {
    }
}