using Autofac;
using Microsoft.Extensions.Configuration;
using PkoAnalizer.Db.Config;
using System;
using System.Linq;

namespace PkoAnalizer.Web.Modules
{
    public class DbConfigsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var configTypes = typeof(IConfig).Assembly.GetTypes()
                .Where(t => t?.Namespace?.StartsWith(typeof(IConfig).Namespace,
                    StringComparison.InvariantCultureIgnoreCase) ?? false);

            foreach (var configType in configTypes)
            {
                builder.Register(c =>
                {
                    var configurationRoot = c.Resolve<IConfigurationRoot>();
                    var configSectionName = configType.Name.Replace("Configuration", "").Replace("Config", "");

                    var configuration = Activator.CreateInstance(configType);
                    configurationRoot.GetSection(configSectionName).Bind(configuration);

                    return configuration;
                })
                .As(configType)
                .SingleInstance();
            }
        }
    }
}
