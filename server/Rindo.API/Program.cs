using Application;
using Rindo.API.Middleware.Exceptions;
using Rindo.Chat;
using Rindo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => 
    { 
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration
    .AddJsonFile("appsettings.db.json", optional: false)
    .AddJsonFile("appsettings.auth.json", optional: false);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy",
        conf => conf
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true)));

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddHttpContextAccessor()
    .AddRepositories()
    .AddApplication();

builder.Services.AddJwt(builder.Configuration);
// builder.Services.AddScoped<AsyncActionAccessFilter>();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.ApplyMigrations();    
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("CorsPolicy"); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();