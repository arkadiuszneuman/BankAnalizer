using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace PkoAnalizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<int> ImportAll()
        {
            return new[] { 1, 2, 4 };
        }
    }
}