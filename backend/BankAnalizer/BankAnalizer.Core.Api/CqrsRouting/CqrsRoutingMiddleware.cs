﻿using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            var endpointResult = endpointsBuilder.GetTypeForEndpoint(context.Request.Path, context.Request.Method);
            if (endpointResult == null)
                return false;

            var userId = context.User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return true;
            }

            var request = await GetRequestText(context.Request);
            var command = GetCommand(endpointResult, request);

            if (command != null)
            {
                SetPatternValuesToCommand(endpointResult, command);

                command.UserId = Guid.Parse(userId);

                var bus = context.RequestServices.GetService<ICommandsBus>();

                _ = bus.SendAsync(endpointResult.Type, command);

                context.Response.StatusCode = StatusCodes.Status202Accepted;
                var returnObject = new { commandId = command.CommandId };
                await context.Response.WriteAsync(returnObject.ToJson());

                return true;
            }

            return false;

            static void SetPatternValuesToCommand(CqrsEndpointsBuilder.EndpointResult endpointResult, Command command)
            {
                foreach (var patternSegmentObject in endpointResult.PatternSegmentObjects)
                {
                    endpointResult.Type.GetProperty(patternSegmentObject.Key).SetValue(command, patternSegmentObject.Value);
                }
            }

            static Command GetCommand(CqrsEndpointsBuilder.EndpointResult endpointResult, string request)
            {
                if (string.IsNullOrEmpty(request))
                    return (Command)Activator.CreateInstance(endpointResult.Type);

                return (Command)JsonSerializer.Deserialize(request, endpointResult.Type, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
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
