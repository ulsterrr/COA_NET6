using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    public class MenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        private const int CacheExpirationSeconds = 300; // 5 minutes

        public MenuService(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            var cacheKey = "menus:all";
            var cachedMenus = await _cacheService.GetAsync<IEnumerable<Menu>>(cacheKey);
            if (cachedMenus != null)
            {
                return cachedMenus;
            }

            var menus = await _unitOfWork.Menus.GetAllAsync();
            await _cacheService.SetAsync(cacheKey, menus, CacheExpirationSeconds);
            return menus;
        }

        public async Task<Menu> GetMenuByIdAsync(int id)
        {
            var cacheKey = $"menu:{id}";
            var cachedMenu = await _cacheService.GetAsync<Menu>(cacheKey);
            if (cachedMenu != null)
            {
                return cachedMenu;
            }

            var menu = await _unitOfWork.Menus.GetByIdAsync(id);
            if (menu != null)
            {
                await _cacheService.SetAsync(cacheKey, menu, CacheExpirationSeconds);
            }
            return menu;
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {
            await _unitOfWork.Menus.AddAsync(menu);
            await _unitOfWork.CompleteAsync();
            await RefreshMenuCache(menu.Id);
            return menu;
        }

        public async Task<bool> UpdateMenuAsync(Menu menu)
        {
            var existingMenu = await _unitOfWork.Menus.GetByIdAsync(menu.Id);
            if (existingMenu == null)
            {
                return false;
            }
            existingMenu.Title = menu.Title;
            existingMenu.Url = menu.Url;
            await _unitOfWork.CompleteAsync();
            await RefreshMenuCache(menu.Id);
            return true;
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var menu = await _unitOfWork.Menus.GetByIdAsync(id);
            if (menu == null)
            {
                return false;
            }
            _unitOfWork.Menus.Delete(menu);
            await _unitOfWork.CompleteAsync();
            await _cacheService.RemoveAsync($"menu:{id}");
            return true;
        }

        private async Task RefreshMenuCache(int menuId)
        {
            await _cacheService.RemoveAsync($"menu:{menuId}");
            var menu = await _unitOfWork.Menus.GetByIdAsync(menuId);
            if (menu != null)
            {
                await _cacheService.SetAsync($"menu:{menuId}", menu, CacheExpirationSeconds);
            }
            await _cacheService.RemoveAsync("menus:all");
        }
    }
}
