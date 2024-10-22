using FactoryManagment.API.Extensions;
using FactoryManagment.Application.Services;
using FactoryManagment.Domain.Interfaces.Auth;
using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Interfaces.Services;
using FactoryManagment.Infrastructure;
using FactoryManagment.Persistence;
using FactoryManagment.Persistence.Repositories;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

services.AddControllers();

services.AddApiAuthentication(configuration);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<FactoryDbContext>();

services.AddHttpContextAccessor();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.Run();