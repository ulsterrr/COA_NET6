using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("tree")]
        public async Task<ActionResult<IEnumerable<object>>> GetDepartmentTree()
        {
            var departmentTree = await _departmentService.GetDepartmentsGroupedByBranchAsync();
            return Ok(departmentTree);
        }
    }
}
