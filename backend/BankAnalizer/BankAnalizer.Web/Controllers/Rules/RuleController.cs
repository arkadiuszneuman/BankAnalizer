using BankAnalizer.Core.Api;
using BankAnalizer.Logic.Rules.Read;
using BankAnalizer.Logic.Rules.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Web.Controllers.Rules
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CommandFilter]
    public class RuleController : BankControllerBase
    {
        private readonly RuleReader ruleReader;

        public RuleController(RuleReader ruleReader)
        {
            this.ruleReader = ruleReader;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<RuleParsedViewModel>> Index() =>
            await ruleReader.ReadRules(GetCurrentUserId());
    }
}
