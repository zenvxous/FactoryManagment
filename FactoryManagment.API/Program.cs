using FactoryManagment.API.Extensions;
using FactoryManagment.Application;
using FactoryManagment.Application.Services;
using FactoryManagment.Domain.Interfaces;
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
services.AddDbContext<LogsDbContext>();

services.AddHttpContextAccessor();


services.AddScoped<IWorkersRepository, WorkersRepository>();
services.AddScoped<IWorkersService, WorkersService>();
services.AddScoped<IUsersRepository, UsersRepository>();
services.AddScoped<IUsersService, UsersService>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<IWorkerRequestIdentifier, WorkerRequestIdentifier>();
services.AddScoped<ILogsRepository, LogsRepository>();

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