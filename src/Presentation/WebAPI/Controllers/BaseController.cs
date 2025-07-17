using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public int? UserId => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        public string CurrentUser => new (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
