using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Jwt;
using Rindo.Infrastructure.Repositories;

namespace Rindo.Infrastructure;

public static class DependencyInjection
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<PostgresDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostgresDbContext>(options =>
            options.UseNpgsql(configuration["POSTGRESQL"],
                b => b.MigrationsAssembly("Rindo.API")));
        services.AddHttpContextAccessor();
        
        services.AddScoped<ProjectRepository>();
        services.AddScoped<IProjectRepository, CachedProjectRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
        services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
}