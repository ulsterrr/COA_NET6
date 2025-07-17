using Application.Features.Roles.Commands;
using Application.Features.Roles.Queries;
using Application.Wrappers.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Infrastructure.Authorization;
using WebAPI.Infrastructure.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [AppAuthorize(PolicyConstants.RoleView)]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetAllRolesQuery()));
        }

        [AppAuthorize(PolicyConstants.RoleView)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetRoleByIdQuery(id)));
        }

        [AppAuthorize(PolicyConstants.RoleCreate)]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [AppAuthorize(PolicyConstants.RoleUpdate)]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(UpdateRoleCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [AppAuthorize(PolicyConstants.RoleDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRole(int id)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new RemoveRoleCommand(id)));
        }

        [AppAuthorize(PolicyConstants.RoleView)]
        [HttpGet("getrolesbyuserid/{userid}")]
        public async Task<IActionResult> GetRolesByUserId(int userid)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetRolesByUserIdQuery(userid)));
        }

        [AppAuthorize(PolicyConstants.RoleUpdate)]
        [HttpPost("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> AddPermissionToRole(int roleId, int permissionId)
        {
            var command = new AddRolePermissionCommand(roleId, permissionId);
            await _mediator.Send(command);
            return NoContent();
        }

        [AppAuthorize(PolicyConstants.RoleUpdate)]
        [HttpDelete("{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromRole(int roleId, int permissionId)
        {
            var command = new RemoveRolePermissionCommand(roleId, permissionId);
            await _mediator.Send(command);
            return NoContent();
        }

        [AppAuthorize(PolicyConstants.RoleView)]
        [HttpGet("{roleId}/permissions")]
        public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
        {
            var query = new GetPermissionsByRoleIdQuery(roleId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
