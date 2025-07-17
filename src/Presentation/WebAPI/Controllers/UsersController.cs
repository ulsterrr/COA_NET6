using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Application.Wrappers.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Infrastructure.Authorization;
using WebAPI.Infrastructure.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AppAuthorize(PolicyConstants.UserUpdate)]
        [HttpPost("quickaddorupdate")]
        public async Task<IActionResult> QuickAddOrUpdateUser([FromBody] QuickAddOrUpdateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.UserView)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetAllUsersWithRolesQuery()));
        }

        [AppAuthorize(PolicyConstants.UserView)]
        [HttpGet("cached")]
        public async Task<IActionResult> GetAllUsersWithRolesCached()
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new GetAllUsersWithRolesWithCacheQuery()));
        }

        [HttpGet("confirmemail/{code}")]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new ConfirmEmailCommand(code)));
        }

        [AppAuthorize(PolicyConstants.UserView)]
        [HttpPut("updateuserrole")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [AppAuthorize(PolicyConstants.UserView)]
        [HttpPut("assignuserroles")]
        public async Task<IActionResult> AssignUserRoles(AssignUserRolesCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [AppAuthorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {
            command.UserId = UserId.Value;
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [AppAuthorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword (ChangePasswordCommand command)
        {
            command.UserId = UserId.Value;
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }
        
        [AppAuthorize]
        [HttpPost("changeemail")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailCommand command)
        {
            command.UserId = UserId.Value;
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(command));
        }

        [HttpGet("resetpassword/{code}/{email}")]
        public async Task<IActionResult> ResetPassword(string code,string email)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new ResetPasswordCommand(code, email)));
        }

        [AppAuthorize(PolicyConstants.UserDelete)]
        [HttpDelete("{userid}")]
        public async Task<IActionResult> DeleteUser(int userid)
        {
            return this.FromResponse<IResponse>(await _mediator.Send(new RemoveUserCommand(userid)));
        }

    }
}
