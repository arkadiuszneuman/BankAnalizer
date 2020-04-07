using BankAnalizer.Core.Api;
using BankAnalizer.Core.Api.CqrsRouting;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static BankAnalizer.Core.Api.CqrsRouting.CqrsEndpointsBuilder;

namespace BankAnalizer.Tests.Core.CqrsRouting
{
    public class CqrsEndpointsBuilderTests
    {
        private class TestCommand1 : Command { }
        private class TestCommand2 : Command { }

        [Fact]
        public void Shoud_match_simple_post_pattern()
        {
            //arrange
            var sut = new CqrsEndpointsBuilder();
            sut.UsePostCommand<TestCommand1>("/api/somepath")
                .UsePostCommand<TestCommand2>("/api/someotherpath");

            //act
            var result = sut.GetTypeForEndpoint("/api/someotherpath", "POST");

            //assert
            result.Should().BeEquivalentTo(new EndpointResult
            {
                Type = typeof(TestCommand2),
                PatternSegmentObjects = new Dictionary<string, object>()
            });
        }

        [Fact]
        public void Shoud_match_simple_delete_pattern()
        {
            //arrange
            var sut = new CqrsEndpointsBuilder();
            sut.UseDeleteCommand<TestCommand1>("/api/somepath")
                .UseDeleteCommand<TestCommand2>("/api/someotherpath");

            //act
            var result = sut.GetTypeForEndpoint("/api/someotherpath", "DELETE");

            //assert
            result.Should().BeEquivalentTo(new EndpointResult
            {
                Type = typeof(TestCommand2),
                PatternSegmentObjects = new Dictionary<string, object>()
            });
        }

        [Fact]
        public void Shoud_match_by_type()
        {
            //arrange
            var sut = new CqrsEndpointsBuilder();
            sut.UsePostCommand<TestCommand1>("/api/someotherpath")
                .UseDeleteCommand<TestCommand2>("/api/someotherpath");

            //act
            var result = sut.GetTypeForEndpoint("/api/someotherpath", "DELETE");

            //assert
            result.Should().BeEquivalentTo(new EndpointResult
            {
                Type = typeof(TestCommand2),
                PatternSegmentObjects = new Dictionary<string, object>()
            });
        }

        [Fact]
        public void Shoud_match_guid_post_pattern()
        {
            //arrange
            var sut = new CqrsEndpointsBuilder();
            sut.UsePostCommand<TestCommand1>("/api/somepath")
                .UsePostCommand<TestCommand2>("/api/someotherpath/{ruleId:Guid}");

            //act
            var result = sut.GetTypeForEndpoint("/api/someotherpath/EC8FD7B7-B82F-4690-BF16-4EFA24B5C649", "POST");

            //assert
            result.Should().BeEquivalentTo(new EndpointResult
            {
                Type = typeof(TestCommand2),
                PatternSegmentObjects = new Dictionary<string, object>
                {
                    { "ruleId", Guid.Parse("EC8FD7B7-B82F-4690-BF16-4EFA24B5C649") }
                }
            });
        }
    }
}
