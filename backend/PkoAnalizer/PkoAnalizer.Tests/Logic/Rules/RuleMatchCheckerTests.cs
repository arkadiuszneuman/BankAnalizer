using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules;
using PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
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
