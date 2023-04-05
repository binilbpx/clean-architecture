using CleanArchitecture.API.StartUp;
using log4net.Config;
using CleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.RegisterServices();

builder.Services.RegisterRepositoryServices();

var app = builder.Build();

app.ConfigureSwagger();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapContactEndpoints();

app.Run();
