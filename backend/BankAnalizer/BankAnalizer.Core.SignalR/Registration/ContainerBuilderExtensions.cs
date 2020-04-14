using Autofac;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Registration;
using BankAnalizer.Core.SignalR.Cqrs;

namespace BankAnalizer.Core.SignalR.Registration
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterSignalrNonGenericExceptionsHandle(this ContainerBuilder builder)
        {
            builder.RegisterCQRS<SignalRCommandsBus>();
            builder.RegisterDecorator<SignalRCommandsBus, ICommandsBus>();
            return builder;
        }
    }
}
