using Application.Features.Roles.Commands;
using Application.Features.Roles.Queries;
using Application.Wrappers.Abstract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [Authorize(Policy = "Role.View")]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetAllRolesQuery()));
        }

        [Authorize(Policy = "Role.View")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetRoleByIdQuery(id)));
        }

        [Authorize(Policy = "Role.Create")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [Authorize(Policy = "Role.Update")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(UpdateRoleCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [Authorize(Policy = "Role.Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRole(int id)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new RemoveRoleCommand(id)));
        }

        [Authorize(Policy = "Role.View")]
        [HttpGet("getrolesbyuserid/{userid}")]
        public async Task<IActionResult> GetRolesByUserId(int userid)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetRolesByUserIdQuery(userid)));
        }
    }
}
