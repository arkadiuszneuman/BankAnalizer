using Autofac;
using PkoAnalizer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }
    }
}
