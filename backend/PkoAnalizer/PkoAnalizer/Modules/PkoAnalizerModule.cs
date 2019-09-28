using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PkoAnalizer.Logic;

namespace PkoAnalizer.Web.Modules
{
    public class PkoAnalizerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(IAnalizerLogic).Assembly)
                .AsImplementedInterfaces();
        }
    }
}
