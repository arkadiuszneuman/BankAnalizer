using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Logic.Read.Rule;
using PkoAnalizer.Logic.Read.Rule.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Rules
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly RuleReader ruleReader;

        public RuleController(RuleReader ruleReader)
        {
            this.ruleReader = ruleReader;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<RuleParsedViewModel>> Index() =>
            await ruleReader.ReadRules();
    }
}
