using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace PkoAnalizer.Web.Controllers
{
    public class BankControllerBase : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            return Guid.Parse(userId);
        }
    }
}
