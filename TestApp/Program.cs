using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PySharpTelegram.Core.Config;
using PySharpTelegram.Core.Extensions;
using PySharpTelegram.Core.Services.AccessGroups;
using Telegram.Bot;
using TestApp.AccessGroups;
using TestApp.Chat;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<BotConfig>(
            context.Configuration.GetSection("BotConfiguration"));
        
        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                var botConfig = sp.GetConfiguration<BotConfig>();
                TelegramBotClientOptions options = new(botConfig.BotToken);
                return new TelegramBotClient(options, httpClient);
            });
        services.ConfigureDefaultPySharpServices();
        services.AddSingletonPySharpChatClasses([typeof(ChatInline), typeof(ChatMessage)]);
        services.AddSingleton<IChatAccessGroup, ChatAccessGroup>();
    })
    .Build();

await host.RunAsync();