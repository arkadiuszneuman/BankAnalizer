using AutofacContrib.NSubstitute;
using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Rules;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using FluentAssertions;
using Xunit;

namespace BankAnalizer.Tests.Logic.Rules
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
            var transaction = new BankTransaction
            {
                Extensions = new LocationExtension
                {
                    Location = "Some test value"
                }.ToJson()
            };

            //act
            var result = sut.IsRuleMatch(rule, transaction);

            //assert
            result.Should().BeTrue();
        }
    }
}
