using Autofac;
using BankAnalizer.Db;

namespace BankAnalizer.Web.Modules
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConnectionFactory>()
                .As<IConnectionFactory>()
                .SingleInstance();

            builder.RegisterType<ContextFactory>()
                .As<IContextFactory>()
                .SingleInstance();

            builder.RegisterType<BankAnalizerContext>()
                .AsSelf();
        }
    }
}
