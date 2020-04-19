using Autofac;
using Autofac.Extensions.DependencyInjection;
using BankAnalizer.Core.Api.CqrsRouting;
using BankAnalizer.Core.Registration;
using BankAnalizer.Core.SignalR;
using BankAnalizer.Core.SignalR.Registration;
using BankAnalizer.Db;
using BankAnalizer.Logic;
using BankAnalizer.Logic.Groups.Commands;
using BankAnalizer.Logic.Rules.Commands;
using BankAnalizer.Logic.Users.UsersConnections.Commands;
using BankAnalizer.Web.StartupConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace BankAnalizer.Web
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
            services.AddCoreSignalR();
            services.AddDbContext<BankAnalizerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                var origins = Configuration.GetSection("AllowedCorsOrigins")
                    .AsEnumerable()
                    .Select(e => e.Value)
                    .Where(e => e != null);
                
                builder
                    .WithOrigins(origins.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterCQRS<IAnalizerLogic>()
                .RegisterSignalrNonGenericExceptionsHandle();

            builder.RegisterAssemblyTypes(typeof(IAnalizerLogic).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterAssemblyModules(this.GetType().Assembly);
        }

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
                    .UseDeleteCommand<RemoveGroupCommand>("/api/transaction/group")
                    .UsePostCommand<SaveRuleCommand>("/api/rule")
                    .UseDeleteCommand<DeleteRuleCommand>("/api/rule/{RuleId:Guid}")
                    .UsePostCommand<RequestUserConnectionCommand>("/api/usersconnection")
                    .UsePostCommand<AcceptConnectionCommand>("/api/usersconnection/accept"))
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<UsersHub>("/hub");
                });

            using (var context = new BankAnalizerContext(app.ApplicationServices.GetRequiredService<DbContextOptions<BankAnalizerContext>>()))
            {
                context.Database.Migrate();
            }
        }
    }
}
