using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        [Authorize(Policy = "Permission.Menu.View")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetAll()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(menus);
        }

        [Authorize(Policy = "Permission.Menu.Create")]
        [HttpPost]
        public async Task<ActionResult<Menu>> Create([FromBody] Menu menu)
        {
            var createdMenu = await _menuService.CreateMenuAsync(menu);
            return CreatedAtAction(nameof(GetById), new { id = createdMenu.Id }, createdMenu);
        }

        [Authorize(Policy = "Permission.Menu.View")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetById(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return Ok(menu);
        }

        [Authorize(Policy = "Permission.Menu.Edit")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }

            var updated = await _menuService.UpdateMenuAsync(menu);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Policy = "Permission.Menu.Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _menuService.DeleteMenuAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
