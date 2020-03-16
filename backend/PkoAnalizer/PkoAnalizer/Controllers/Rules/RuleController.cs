using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Rules;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Logic.Read.Rule;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Rules
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            await ruleReader.ReadRules();

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save([FromHeader]string connectionId, RuleParsedViewModel rule)
        {
            
            var command = new SaveRuleCommand(connectionId, GetCurrentUserId(), rule);
            _ = bus.Send(command);
            return Accepted(command);
        }

        [HttpDelete]
        [Route("{ruleId:Guid}")]
        public async Task<ActionResult> Delete([FromHeader]string connectionId, Guid ruleId)
        {
            var command = new DeleteRuleCommand(connectionId, ruleId);
            _ = bus.Send(command);
            return Accepted(command);
        }
    }
}
