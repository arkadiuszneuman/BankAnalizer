using Autofac;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Logic.Transactions.Import.Commands.Handlers;
using System;
using System.Linq;

namespace BankAnalizer.Web.Modules.Cqrs
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(ImportCommandHandler).Assembly)
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
    }
}
