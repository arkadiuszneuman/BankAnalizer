using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Rules;
using BankAnalizer.Logic.Rules.ViewModels;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Rules
{
    public class RuleParserTests : BaseUnitTest<RuleParser>
    {
        [Fact]
        public void Should_parse_contains_rule()
        {
            // arrange
            var ruleViewModel = new RuleViewModel
            {
                Id = Guid.Parse("C61308F9-AB25-4123-9084-BCDA14F2540F"),
                RuleDefinition = "Title Contains TESCO AND TESCO",
                GroupName = "Some Group",
                RuleName = "Some rule name"
            };

            //act
            var result = Sut.Parse(ruleViewModel);

            //assert
            result.Should().BeEquivalentTo(new ParsedRule
            {
                Id = Guid.Parse("C61308F9-AB25-4123-9084-BCDA14F2540F"),
                BankTransactionTypeId = null,
                Column = "Title",
                IsColumnInExtensions = false,
                RuleType = RuleType.Contains,
                Value = "TESCO AND TESCO",
                GroupName = "Some Group",
                RuleName = "Some rule name"
            });
        }

        [Fact]
        public void Should_parse_contains_rule_and_extension_column()
        {
            // arrange
            var ruleViewModel = new RuleViewModel
            {
                BankTransactionTypeId = Guid.NewGuid(),
                RuleDefinition = "Extensions.Location Contains TESCO"
            };

            //act
            var result = Sut.Parse(ruleViewModel);

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
            var ruleViewModel = new RuleViewModel
            {
                BankTransactionTypeId = Guid.NewGuid(),
                RuleDefinition = "Extensions.Location Contains "
            };

            //act
            var result = Sut.Parse(ruleViewModel.AsList());

            //assert
            result.Should().BeEmpty();
        }
    }
}
