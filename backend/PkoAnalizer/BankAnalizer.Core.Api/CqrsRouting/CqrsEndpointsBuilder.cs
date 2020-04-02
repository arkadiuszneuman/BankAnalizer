using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.IO;

namespace BankAnalizer.Core.Api.CqrsRouting
{
    public interface ICqrsEndpointsBuilder
    {
        CqrsEndpointsBuilder UseCommand<T>(string path) where T : ICommand;
    }

    public class CqrsEndpointsBuilder : ICqrsEndpointsBuilder
    {
        private Dictionary<string, Type> endpoints = new Dictionary<string, Type>();

        public CqrsEndpointsBuilder UseCommand<T>(string path)
            where T : ICommand
        {
            endpoints.Add(path, typeof(T));
            return this;
        }

        public Type GetTypeForEndpoint(string endpoint)
        {
            if (endpoints.ContainsKey(endpoint))
                return endpoints[endpoint];

            return null;
        }
    }
}
