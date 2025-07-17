using Application.Features.Departments.Commands;
using Application.Features.Departments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Infrastructure.Authorization;

namespace Presentation.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : BaseController
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AppAuthorize(PolicyConstants.DepartmentUpdate)]
        [HttpPost("quickaddorupdate")]
        public async Task<IActionResult> QuickAddOrUpdateDepartment([FromBody] QuickAddOrUpdateDepartmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.DepartmentView)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllDepartmentsQuery());
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.DepartmentCreate)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.DepartmentUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Id mismatch");
            }
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.DepartmentDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new RemoveDepartmentCommand { Id = id });
            return Ok(response);
        }
    }
}
