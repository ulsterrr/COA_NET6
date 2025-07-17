using Application.Features.Organization.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Infrastructure.Authorization;

namespace Presentation.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : BaseController
    {
        private readonly IMediator _mediator;

        public OrganizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AppAuthorize(PolicyConstants.OrganizationView)]
        [HttpGet("tree")]
        public async Task<IActionResult> GetOrganizationTree()
        {
            var tree = await _mediator.Send(new GetOrganizationTreeQuery());
            return Ok(tree);
        }
    }
}
