using Microsoft.AspNetCore.Mvc;

namespace BankAnalizer.Web.Controllers
{
    [Route("")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        public string Index()
        {
            return "Hello from BankAnalizer.Web";
        }
    }
}