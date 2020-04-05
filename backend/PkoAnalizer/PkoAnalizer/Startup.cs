using Autofac;
using Autofac.Extensions.DependencyInjection;
using BankAnalizer.Core.Api.CqrsRouting;
using BankAnalizer.Infrastructure.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Logic;
using PkoAnalizer.Logic.Import.Hubs;
using PkoAnalizer.Web.Startup;

namespace PkoAnalizer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddJsonFile($"appsettings.Secrets.json", optional: true)
               .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationWithBearerToken(Configuration);
            services.AddControllers();
            services.AddOptions();
            services.AddSignalR();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IAnalizerLogic).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterAssemblyModules(this.GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseCqrsEndpointsCommands(endpoints => endpoints
                    .UsePostCommand<AddGroupCommand>("/api/transaction/group")
                    .UsePostCommand<SaveRuleCommand>("/api/rule"))
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<SendSignalRAnswerHub>("/hub");
                });
        }
    }
}
