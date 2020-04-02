using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankAnalizer.Core.Api.CqrsRouting
{
    internal class CqrsRoutingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly CqrsEndpointsBuilder endpointsBuilder;

        public CqrsRoutingMiddleware(RequestDelegate next, CqrsEndpointsBuilder endpointsBuilder)
        {
            this.next = next;
            this.endpointsBuilder = endpointsBuilder;
        }

        public async Task Invoke(HttpContext context)
        {
            if (await InvokeRequestedCommand(context))
                return;

            await next(context);
        }

        public async Task<bool> InvokeRequestedCommand(HttpContext context)
        {
            if (context.Request.Method != "POST")
                return false;

            var type = endpointsBuilder.GetTypeForEndpoint(context.Request.Path);
            if (type == null)
                return false;

            var request = await GetRequestText(context.Request);
            var command = (Command)JsonSerializer.Deserialize(request, type, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            if (command != null)
            {
                var connectionId = context.Request.Headers["connectionId"].FirstOrDefault();
                var userId = context.User.FindFirstValue(ClaimTypes.Name);

                command.ConnectionId = connectionId;
                command.UserId = Guid.Parse(userId);

                var bus = context.RequestServices.GetService<ICommandsBus>();

                _ = bus.SendAsync(type, command);

                context.Response.StatusCode = StatusCodes.Status202Accepted;
                var returnObject = new { commandId = command.CommandId };
                await context.Response.WriteAsync(returnObject.ToJson());

                return true;
            }

            return false;
        }

        private async Task<string> GetRequestText(HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body = body;

            return bodyAsText;
        }
    }
}
