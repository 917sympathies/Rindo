using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rindo.Infrastructure.Jwt;
using Rindo.Infrastructure.Repositories;
using Rindo.Infrastructure.Repositories.Cached;

namespace Rindo.Infrastructure;

public record ConnectionStrings(string POSTGRESQL, string REDIS, string RABBITMQ);

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
        var dbOptions = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
        if (dbOptions is null)
        {
            throw new InvalidOperationException("You must provide a connection string in configuration");
        }
        services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(dbOptions.POSTGRESQL, b => b.MigrationsAssembly("Rindo.API")));
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = dbOptions.REDIS;
        });
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ProjectRepository>();
        services.AddScoped<IProjectRepository, CachedProjectRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
        services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        return services;
    }
}