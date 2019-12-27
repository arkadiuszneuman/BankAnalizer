using AutofacContrib.NSubstitute;
using PkoAnalizer.Logic.Rules;
using PkoAnalizer.Logic.Rules.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PkoAnalizer.Core.ExtensionMethods;
using FluentAssertions;

namespace PkoAnalizer.Tests.Logic.Rules
{
    public class RuleParserTests
    {
        [Fact]
        public void Should_parse_contains_rule()
        {
            // arrange
            var sut = new AutoSubstitute().Resolve<RuleParser>();
            var ruleViewModel = new RuleViewModel
            {
                RuleDefinition = "Title Contains TESCO"
            };

            //act
            var result = sut.Parse(ruleViewModel.AsList());

            //assert
            result.Should().BeEquivalentTo(new ParsedRule
            {
                BankTransactionTypeId = null,
                Column = "Title",
                IsColumnInExtensions = false,
                RuleType = RuleType.Contains,
                Value = "TESCO"
            });
        }

        [Fact]
        public void Should_parse_contains_rule_and_extension_column()
        {
            // arrange
            var sut = new AutoSubstitute().Resolve<RuleParser>();
            var ruleViewModel = new RuleViewModel {
                BankTransactionTypeId = Guid.NewGuid(),
                RuleDefinition = "Extensions.Location Contains TESCO"
            };

            //act
            var result = sut.Parse(ruleViewModel.AsList());

            //assert
            result.Should().BeEquivalentTo(new ParsedRule
            {
                BankTransactionTypeId = ruleViewModel.BankTransactionTypeId,
                Column = "Location",
                IsColumnInExtensions = true,
                RuleType = RuleType.Contains,
                Value = "TESCO",
            });
        }

        [Fact]
        public void Should_return_null_if_invalid_rule()
        {
            // arrange
            var sut = new AutoSubstitute().Resolve<RuleParser>();
            var ruleViewModel = new RuleViewModel
            {
                BankTransactionTypeId = Guid.NewGuid(),
                RuleDefinition = "Extensions.Location Contains "
            };

            //act
            var result = sut.Parse(ruleViewModel.AsList());

            //assert
            result.Should().BeEmpty();
        }
    }
}
