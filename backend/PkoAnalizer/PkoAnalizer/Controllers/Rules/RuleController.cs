using BankAnalizer.Core.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Logic.Rules.Read;
using PkoAnalizer.Logic.Rules.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Rules
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CommandFilter]
    public class RuleController : BankControllerBase
    {
        private readonly ICommandsBus bus;
        private readonly RuleReader ruleReader;

        public RuleController(ICommandsBus bus,
            RuleReader ruleReader)
        {
            this.bus = bus;
            this.ruleReader = ruleReader;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<RuleParsedViewModel>> Index() =>
            await ruleReader.ReadRules(GetCurrentUserId());
    }
}
