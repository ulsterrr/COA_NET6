,using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Policy = "Permission.Role.View")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [Authorize(Policy = "Permission.Role.Create")]
        [HttpPost]
        public async Task<ActionResult<Role>> Create([FromBody] Role role)
        {
            var createdRole = await _roleService.CreateRoleAsync(role);
            return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
        }

        [Authorize(Policy = "Permission.Role.View")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [Authorize(Policy = "Permission.Role.Edit")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            var updated = await _roleService.UpdateRoleAsync(role);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Policy = "Permission.Role.Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roleService.DeleteRoleAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Policy = "Permission.Role.Edit")]
        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AssignPermissions(int id, [FromBody] List<int> permissionIds)
        {
            var result = await _roleService.AssignPermissionsToRoleAsync(id, permissionIds);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Policy = "Permission.Role.View")]
        [HttpGet("permissions-by-menu")]
        public async Task<ActionResult<IEnumerable<object>>> GetRolePermissionsByMenu()
        {
            var tree = await _roleService.GetRolePermissionsByMenuAsync();
            return Ok(tree);
        }
    }
}
