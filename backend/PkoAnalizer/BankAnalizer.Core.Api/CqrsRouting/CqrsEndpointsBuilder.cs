using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.IO;

namespace BankAnalizer.Core.Api.CqrsRouting
{
    public interface ICqrsEndpointsBuilder
    {
        CqrsEndpointsBuilder UsePostCommand<T>(string path) where T : ICommand;
    }

    public class CqrsEndpointsBuilder : ICqrsEndpointsBuilder
    {
        private readonly Dictionary<(string, string), Type> endpoints = new Dictionary<(string, string), Type>();

        public CqrsEndpointsBuilder UsePostCommand<T>(string path)
            where T : ICommand
        {
            endpoints.Add((path, "POST"), typeof(T));
            return this;
        }

        public CqrsEndpointsBuilder UseDeleteCommand<T>(string path)
            where T : ICommand
        {
            endpoints.Add((path, "DELETE"), typeof(T));
            return this;
        }

        public Type GetTypeForEndpoint(string endpoint, string method)
        {
            endpoints.TryGetValue((endpoint, method), out Type type);
            return type;
        }
    }
}
