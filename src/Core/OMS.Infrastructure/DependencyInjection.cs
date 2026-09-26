using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Interfaces;
using OMS.Infrastructure.Persistence;
using OMS.Infrastructure.Services;

namespace OMS.Infrastructure;

/// <summary>
/// Single place OMS.Web calls into to wire up this whole layer (DbContext + every
/// service implementation). Adding a new feature means adding one AddScoped line here
/// — nothing in OMS.Web's Program.cs needs to change.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDepartmentMergeService, DepartmentMergeService>();

        return services;
    }
}
