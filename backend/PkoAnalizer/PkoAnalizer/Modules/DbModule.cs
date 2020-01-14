using Autofac;
using PkoAnalizer.Db;

namespace PkoAnalizer.Web.Modules
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConnectionFactory>()
                .SingleInstance()
                .AsSelf();

            builder.RegisterType<ContextFactory>()
                .As<IContextFactory>()
                .SingleInstance();

            builder.RegisterType<PkoContext>()
                .AsSelf();
        }
    }
}
