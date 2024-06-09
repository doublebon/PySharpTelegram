using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PySharpTelegram.Core.Handlers;
using PySharpTelegram.Core.Services.Abstract;
using PySharpTelegram.Core.Services.AccessGroups;

namespace PySharpTelegram.Core.Extensions;

public static class Extensions
{
    public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
        where T : class
    {
        var o = serviceProvider.GetService<IOptions<T>>();
        if (o is null)
            throw new ArgumentNullException(nameof(T));

        return o.Value;
    }
    
    public static IServiceCollection ConfigureDefaultPySharpServices(this IServiceCollection services)
    {
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
        services.AddSingleton<MessageAttributesHandler>();
        services.AddSingleton<InlineAttributesHandler>();
        return services;
    }
    
    
    public static IServiceCollection AddSingletonPySharpChatClasses(this IServiceCollection services, Type[] classesWithChatMethods)
    {
        services.AddSingleton(ChatClassesConnector.Create(classesWithChatMethods));
        return services;
    }
}