using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rindo.Chat;
using Rindo.Infrastructure.DependencyInjection;
using Rindo.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy", 
        conf => conf.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(x => true)));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ApplyMigrations();   
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();