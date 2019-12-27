using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;
using PkoAnalizer.Logic.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Rules
{
    public class RuleMatchCheckerTests
    {
        [Fact]
        public void Should_match_extension_column()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<RuleMatchChecker>();
            var rule = new ParsedRule
            {
                IsColumnInExtensions = true,
                Column = "Location",
                RuleType = RuleType.Contains,
                Value = "Test value"
            };
            var transaction = new PkoTransaction
            {
                Extensions = new LocationExtension
                {
                    Location = "Some Test value"
                }.ToJson()
            };

            //act
            var result = sut.IsRuleMatch(rule, transaction);

            //assert
            result.Should().BeTrue();
        }
    }
}
