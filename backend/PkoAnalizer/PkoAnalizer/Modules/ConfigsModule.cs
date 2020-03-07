using Autofac;
using Microsoft.Extensions.Configuration;
using PkoAnalizer.Db.Config;
using PkoAnalizer.Logic.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PkoAnalizer.Web.Modules
{
    public class ConfigsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var configTypes = GetConfigsFromType<ILogicConfig>()
                .Union(GetConfigsFromType<IDbConfig>());

            foreach (var configType in configTypes)
            {
                builder.Register(c =>
                {
                    var configurationRoot = c.Resolve<IConfigurationRoot>();
                    var configSectionName = configType.Name.Replace("Configuration", "").Replace("Config", "");

                    return configurationRoot.GetSection(configSectionName).Get(configType);
                })
                .As(configType)
                .SingleInstance();
            }
        }

        private static IEnumerable<Type> GetConfigsFromType<T>()
        {
            return typeof(T).Assembly.GetTypes()
                            .Where(t => t?.Namespace?.StartsWith(typeof(T).Namespace,
                                StringComparison.InvariantCultureIgnoreCase) ?? false);
        }
    }
}
