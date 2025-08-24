using System.Text;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rindo.Domain.Services;
using Rindo.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStageService, StageService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        if (jwtOptions is null)
        {
            throw new InvalidOperationException("You haven't set JWT settings in configuration file");
        }
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[jwtOptions.CookiesName];
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        return services;
    }
}