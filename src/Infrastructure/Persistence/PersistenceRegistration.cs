using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;


namespace Persistence
{
    public static class PersistenceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CAContext>(options =>
            {
                //options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            });
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        }
    }
}