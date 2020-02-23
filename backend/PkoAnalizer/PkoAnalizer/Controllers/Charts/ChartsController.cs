using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Logic.Read.Charts.Groups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Charts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly GroupsReader groupsReader;

        public ChartsController(GroupsReader groupsReader)
        {
            this.groupsReader = groupsReader;
        }

        [HttpGet]
        [Route("Groups")]
        public async Task<IEnumerable<GroupsViewModel>> GetGroups() =>
            await groupsReader.GetGroups();
    }
}
