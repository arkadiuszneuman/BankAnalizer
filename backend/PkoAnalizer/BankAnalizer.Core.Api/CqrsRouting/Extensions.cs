using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Core.Api.CqrsRouting
{
    public static class Extensions
    {
        public static IApplicationBuilder UseCqrsEndpointsCommands(this IApplicationBuilder applicationBuilder, Action<ICqrsEndpointsBuilder> endpointsAction)
        {
            var endpointsBuilder = new CqrsEndpointsBuilder();
            endpointsAction(endpointsBuilder);

            applicationBuilder.UseMiddleware<CqrsRoutingMiddleware>(endpointsBuilder);

            return applicationBuilder;
        }
    }
}
