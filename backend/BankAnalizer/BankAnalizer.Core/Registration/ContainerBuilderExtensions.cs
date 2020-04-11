using Autofac;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using System;
using System.Collections.Generic;

namespace BankAnalizer.Core.Registration
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCQRS<TAssemblyType>(this ContainerBuilder builder)
        {
            RegisterCommands<TAssemblyType>(builder);
            RegisterEvents<TAssemblyType>(builder);

            return builder;
        }

        private static void RegisterCommands<TAssemblyType>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(TAssemblyType).Assembly)
                .Where(x => x.IsAssignableTo<ICommandHandler>())
                .AsImplementedInterfaces();

            builder.Register<Func<Type, ICommandHandler>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();

                return t =>
                {
                    var handlerType = typeof(ICommandHandler<>).MakeGenericType(t);
                    if (ctx.TryResolve(handlerType, out object resolvedType))
                        return (ICommandHandler)resolvedType;

                    return null;
                };
            });

            builder.RegisterType<CommandsBus>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterDecorator<LoggerCommandsBus, ICommandsBus>();
        }

        private static void RegisterEvents<TAssemblyType>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(TAssemblyType).Assembly)
                            .Where(x => x.IsAssignableTo<IHandleEvent>())
                            .AsImplementedInterfaces();

            builder.Register<Func<Type, IEnumerable<IHandleEvent>>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return t =>
                {
                    var handlerType = typeof(IHandleEvent<>).MakeGenericType(t);
                    var handlersCollectionType = typeof(IEnumerable<>).MakeGenericType(handlerType);
                    return (IEnumerable<IHandleEvent>)ctx.Resolve(handlersCollectionType);
                };
            }).SingleInstance();

            builder.RegisterType<EventsBus>()
                .AsImplementedInterfaces();
        }
    }
}
