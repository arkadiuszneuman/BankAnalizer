using Autofac;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using System;
using System.Collections.Generic;

namespace BankAnalizer.Core.Registration
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCQRS<ASSEMBLYTYPE>(this ContainerBuilder builder)
        {
            RegisterCommands<ASSEMBLYTYPE>(builder);
            RegisterEvents<ASSEMBLYTYPE>(builder);

            return builder;
        }

        private static void RegisterCommands<ASSEMBLYTYPE>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ASSEMBLYTYPE).Assembly)
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
        }

        private static void RegisterEvents<ASSEMBLYTYPE>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ASSEMBLYTYPE).Assembly)
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
