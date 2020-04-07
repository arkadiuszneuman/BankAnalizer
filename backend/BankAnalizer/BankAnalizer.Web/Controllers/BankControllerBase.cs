using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BankAnalizer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankControllerBase : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            return Guid.Parse(userId);
        }
    }
}
