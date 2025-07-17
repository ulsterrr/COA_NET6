using Application.Features.Branches.Commands;
using Application.Features.Branches.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Infrastructure.Authorization;
using WebAPI.Controllers;
using WebAPI.Infrastructure.Authorization;

namespace Presentation.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : BaseController
    {
        private readonly IMediator _mediator;

        public BranchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AppAuthorize(PolicyConstants.BranchUpdate)]
        [HttpPost("quickaddorupdate")]
        public async Task<IActionResult> QuickAddOrUpdateBranch([FromBody] QuickAddOrUpdateBranchCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.BranchView)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllBranchesQuery());
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.BranchCreate)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.BranchUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Id mismatch");
            }
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AppAuthorize(PolicyConstants.BranchDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new RemoveBranchCommand { Id = id });
            return Ok(response);
        }
    }
}
