using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Core.Interfaces;
using Infrastructure.Cache;
using Moq;
using Xunit;

namespace Tests.Core.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cacheServiceMock = new Mock<ICacheService>();
            _roleService = new RoleService(_unitOfWorkMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task CreateRoleAsync_ShouldAddRoleAndRefreshCache()
        {
            var role = new Role { Id = 1, Name = "Admin" };
            _unitOfWorkMock.Setup(u => u.Roles.AddAsync(role)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(0);
            _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<string>(), role, It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _roleService.CreateRoleAsync(role);

            _unitOfWorkMock.Verify(u => u.Roles.AddAsync(role), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync($"role:{role.Id}"), Times.Once);
            _cacheServiceMock.Verify(c => c.SetAsync($"role:{role.Id}", role, It.IsAny<int>()), Times.Once);
            Assert.Equal(role, result);
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnCachedRole_WhenCacheExists()
        {
            var role = new Role { Id = 1, Name = "Admin" };
            _cacheServiceMock.Setup(c => c.GetAsync<Role>($"role:{role.Id}")).ReturnsAsync(role);

            var result = await _roleService.GetRoleByIdAsync(role.Id);

            _cacheServiceMock.Verify(c => c.GetAsync<Role>($"role:{role.Id}"), Times.Once);
            _unitOfWorkMock.Verify(u => u.Roles.GetByIdAsync(It.IsAny<int>()), Times.Never);
            Assert.Equal(role, result);
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnRoleFromDbAndCache_WhenCacheMiss()
        {
            var role = new Role { Id = 1, Name = "Admin" };
            _cacheServiceMock.Setup(c => c.GetAsync<Role>($"role:{role.Id}")).ReturnsAsync((Role)null);
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(role.Id)).ReturnsAsync(role);
            _cacheServiceMock.Setup(c => c.SetAsync($"role:{role.Id}", role, It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _roleService.GetRoleByIdAsync(role.Id);

            _unitOfWorkMock.Verify(u => u.Roles.GetByIdAsync(role.Id), Times.Once);
            _cacheServiceMock.Verify(c => c.SetAsync($"role:{role.Id}", role, It.IsAny<int>()), Times.Once);
            Assert.Equal(role, result);
        }

        [Fact]
        public async Task UpdateRoleAsync_ShouldReturnFalse_WhenRoleNotFound()
        {
            var role = new Role { Id = 1, Name = "Admin" };
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(role.Id)).ReturnsAsync((Role)null);

            var result = await _roleService.UpdateRoleAsync(role);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateRoleAsync_ShouldUpdateRoleAndRefreshCache_WhenRoleExists()
        {
            var role = new Role { Id = 1, Name = "Admin" };
            var existingRole = new Role { Id = 1, Name = "User" };
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(role.Id)).ReturnsAsync(existingRole);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(0);
            _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Role>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _roleService.UpdateRoleAsync(role);

            Assert.True(result);
            Assert.Equal(role.Name, existingRole.Name);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync($"role:{role.Id}"), Times.Once);
            _cacheServiceMock.Verify(c => c.SetAsync($"role:{role.Id}", existingRole, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnFalse_WhenRoleNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(1)).ReturnsAsync((Role)null);

            var result = await _roleService.DeleteRoleAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldDeleteRoleAndRemoveCache_WhenRoleExists()
        {
            var role = new Role { Id = 1, Name = "Admin" };
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(role.Id)).ReturnsAsync(role);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(0);
            _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _roleService.DeleteRoleAsync(role.Id);

            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.Roles.Delete(role), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync($"role:{role.Id}"), Times.Once);
        }

        [Fact]
        public async Task AssignPermissionsToRoleAsync_ShouldReturnFalse_WhenRoleNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(1)).ReturnsAsync((Role)null);

            var result = await _roleService.AssignPermissionsToRoleAsync(1, new List<int>());

            Assert.False(result);
        }

        [Fact]
        public async Task AssignPermissionsToRoleAsync_ShouldAssignPermissionsAndRefreshCache_WhenRoleExists()
        {
            var role = new Role { Id = 1, Name = "Admin", Permissions = new List<Permission>() };
            var permission = new Permission { Id = 1, Name = "Permission1" };
            _unitOfWorkMock.Setup(u => u.Roles.GetByIdAsync(role.Id)).ReturnsAsync(role);
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(permission.Id)).ReturnsAsync(permission);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(0);
            _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Role>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _roleService.AssignPermissionsToRoleAsync(role.Id, new List<int> { permission.Id });

            Assert.True(result);
            Assert.Contains(permission, role.Permissions);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveAsync($"role:{role.Id}"), Times.Once);
            _cacheServiceMock.Verify(c => c.SetAsync($"role:{role.Id}", role, It.IsAny<int>()), Times.Once);
        }
    }
}
