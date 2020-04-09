using Microsoft.Extensions.DependencyInjection;

namespace BankAnalizer.Core.SignalR
{
    public static class SignalrExtensions
    {
        public static IServiceCollection AddCoreSignalR(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISignalrClient, SignalrClient>();

            serviceCollection.AddSignalR();
            return serviceCollection;
        }
    }
}
