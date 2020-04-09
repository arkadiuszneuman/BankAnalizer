using BankAnalizer.Core.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace BankAnalizer.Core.Api
{
    public class CommandFilter : Attribute, IActionFilter
    {
        Command command;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (command != null)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status202Accepted;
                var returnObject = new { commandId = command.CommandId };
                context.HttpContext.Response.WriteAsync(returnObject.ToJson());
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<CommandFilter>>();
            command = context.ActionArguments.Select(x => x.Value).OfType<Command>().FirstOrDefault();

            if (command != null)
            {
                var connectionId = context.HttpContext.Request.Headers["connectionId"].FirstOrDefault();
                var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.Name);

                command.UserId = Guid.Parse(userId);
            }
        }
    }
}
