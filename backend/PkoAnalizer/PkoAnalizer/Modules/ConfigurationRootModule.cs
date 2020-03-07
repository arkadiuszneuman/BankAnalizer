using Autofac;
using Microsoft.Extensions.Configuration;
using System;

namespace PkoAnalizer.Web.Modules
{
    public class ConfigurationRootModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c =>
            {
                var environment = Environment.GetEnvironmentVariable("WRAPPER_ENVIRONMENT");

                var configurationBuilder = new ConfigurationBuilder();

                configurationBuilder.AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.Secrets.json", optional: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddJsonFile($"appsettings.Secrets.json", optional: true)
                    .AddEnvironmentVariables();

                return configurationBuilder.Build();
            })
            .SingleInstance();
        }
    }
}
