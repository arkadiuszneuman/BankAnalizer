using Autofac;
using PkoAnalizer.Core.Cqrs.Event;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PkoAnalizer.Web.Modules.Cqrs
{
    public class EventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly)
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
