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

        [AppAuthorize(PolicyConstants.UserUpdate)]
        [HttpPost("grantfullpermissions")]
        public async Task<IActionResult> GrantFullPermissions([FromBody] int userId)
        {
            var result = await _mediator.Send(new GrantFullPermissionsCommand { UserId = userId });
            if (result)
            {
                return Ok(new { message = "Full permissions granted successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to grant full permissions. User may not have roles assigned." });
            }
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

        [AppAuthorize(PolicyConstants.UserUpdate)]
        [HttpPost("adduserpermissions")]
        public async Task<IActionResult> AddUserPermissions([FromBody] AddUserPermissionCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "User permissions added successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to add user permissions." });
            }
        }

        [AppAuthorize(PolicyConstants.UserUpdate)]
        [HttpPost("removeuserpermissions")]
        public async Task<IActionResult> RemoveUserPermissions([FromBody] RemoveUserPermissionCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "User permissions removed successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to remove user permissions." });
            }
        }

        [AppAuthorize(PolicyConstants.UserView)]
        [HttpGet("userpermissions/{userId}")]
        public async Task<IActionResult> GetUserPermissions(int userId)
        {
            var permissions = await _mediator.Send(new Application.Features.Users.Queries.GetUserPermissionsQuery { UserId = userId });
            return Ok(permissions);
        }
    }
}
