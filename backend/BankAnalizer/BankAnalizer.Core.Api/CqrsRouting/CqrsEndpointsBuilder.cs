using BankAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BankAnalizer.Core.Api.CqrsRouting
{
    public interface ICqrsEndpointsBuilder
    {
        CqrsEndpointsBuilder UsePostCommand<T>(string path) where T : ICommand;
    }

    public class CqrsEndpointsBuilder : ICqrsEndpointsBuilder
    {
        public class EndpointResult
        {
            public Type Type { get; set; }
            public IReadOnlyDictionary<string, object> PatternSegmentObjects { get; set; }
        }

        private readonly List<(string endpoint, string method, Type type)> endpoints
            = new List<(string endpoint, string method, Type type)>();

        public CqrsEndpointsBuilder UsePostCommand<T>(string path)
            where T : ICommand
        {
            endpoints.Add((path, "POST", typeof(T)));
            return this;
        }

        public CqrsEndpointsBuilder UseDeleteCommand<T>(string path)
            where T : ICommand
        {
            endpoints.Add((path, "DELETE", typeof(T)));
            return this;
        }

        public EndpointResult GetTypeForEndpoint(string endpoint, string method)
        {
            var matchedEndpoint = endpoints
                .Where(e => e.method == method)
                .Select(e => (type: e.type, endpointResult: AnalizeEndpoint(e.endpoint, endpoint)))
                .SingleOrDefault(e => e.endpointResult.isMatch);

            if (matchedEndpoint.type == null)
                return null;

            return new EndpointResult { Type = matchedEndpoint.type, PatternSegmentObjects = matchedEndpoint.endpointResult.convertedSegments };
        }

        private (bool isMatch, Dictionary<string, object> convertedSegments) AnalizeEndpoint(string endpointPattern, string endpointToMatch)
        {
            var endpointPatternSplitted = endpointPattern.Split("/");
            var endpointToMatchSplitted = endpointToMatch.Split("/");

            if (endpointPatternSplitted.Length != endpointToMatchSplitted.Length)
                return (false, null);

            var convertedSegments = new Dictionary<string, object>();

            for (int i = 0; i < endpointToMatchSplitted.Length; i++)
            {
                var patternSegment = endpointPatternSplitted[i];
                var matchSegment = endpointToMatchSplitted[i];

                if (patternSegment.StartsWith('{') && patternSegment.EndsWith('}'))
                {
                    var index = patternSegment.LastIndexOf(':') + 1;
                    var typeString = patternSegment[index..^1];
                    var type = Type.GetType("System." + typeString);

                    var convertedType = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(matchSegment);
                    if (convertedType == null)
                        return (false, null);
                    else
                    {
                        var segmentName = patternSegment[1..(index - 1)];
                        convertedSegments.Add(segmentName, convertedType);
                    }

                    continue;
                }

                if (patternSegment != matchSegment)
                    return (false, null);
            }

            return (true, convertedSegments);
        }
    }
}
