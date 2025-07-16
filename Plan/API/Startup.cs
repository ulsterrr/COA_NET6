using System.Text;
using Core.Interfaces;
using Infrastructure.Auth;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Data;
using System.Data.SqlClient;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = SerilogLogger.CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IDbConnection>(sp =>
                new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, JwtAuthService>();
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddScoped<Core.Services.PermissionService>();
            services.AddScoped<Core.Services.UserService>();

            services.AddHttpContextAccessor();

            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Register authorization policies dynamically from database
            var serviceProvider = services.BuildServiceProvider();
            var permissionService = serviceProvider.GetService<Core.Services.PermissionService>();
            var permissions = permissionService.GetAllPermissionsAsync().GetAwaiter().GetResult();

            services.AddAuthorization(options =>
            {
                foreach (var permission in permissions)
                {
                    options.AddPolicy("Permission." + permission.Name, policy =>
                        policy.Requirements.Add(new Infrastructure.Authorization.PermissionRequirement(permission.Name)));
                }
            });

            // Register the permission authorization handler
            services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Infrastructure.Authorization.PermissionAuthorizationHandler>();

            // Register the organization authorization handler
            services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Infrastructure.Authorization.OrganizationAuthorizationHandler>();

            // Register authorization policies dynamically from database for organization requirements
            var branchNames = permissionService.GetAllBranchesAsync().GetAwaiter().GetResult();
            var departmentNames = permissionService.GetAllDepartmentsAsync().GetAwaiter().GetResult();

            services.AddAuthorization(options =>
            {
                foreach (var branch in branchNames)
                {
                    foreach (var department in departmentNames)
                    {
                        var policyName = $"Organization.{branch}.{department}";
                        options.AddPolicy(policyName, policy =>
                            policy.Requirements.Add(new Infrastructure.Authorization.OrganizationRequirement(branch, department)));
                    }
                }
            });
        }

        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
