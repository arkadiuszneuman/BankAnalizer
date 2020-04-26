using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Rules;
using FluentAssertions;
using Xunit;

namespace BankAnalizer.Tests.Logic.Rules
{
    public class RuleMatchCheckerTests : BaseUnitTest<RuleMatchChecker>
    {
        [Fact]
        public void Should_match_extension_column()
        {
            //arrange
            var rule = new ParsedRule
            {
                IsColumnInExtensions = true,
                Column = "Location",
                RuleType = RuleType.Contains,
                Value = "Test value"
            };
            var transaction = new BankTransaction
            {
                Extensions = new
                {
                    Location = "Some test value"
                }.ToJson()
            };

            //act
            var result = Sut.IsRuleMatch(rule, transaction);

            //assert
            result.Should().BeTrue();
        }
    }
}
