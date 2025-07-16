using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = "Permission.User.View")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Policy = "Permission.User.Edit")]
        [HttpPost("{userId}/roles")]
        public async Task<ActionResult> AssignRoles(int userId, [FromBody] List<int> roleIds)
        {
            var result = await _userService.AssignRolesToUserAsync(userId, roleIds);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
