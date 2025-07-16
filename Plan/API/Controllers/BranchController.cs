using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly BranchService _branchService;

        public BranchController(BranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet("tree")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranchTree()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return Ok(branches);
        }
    }
}
