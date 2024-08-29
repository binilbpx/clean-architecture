using CleanArchitecture.API.StartUp;
using log4net.Config;
using CleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.RegisterServices();

builder.Services.RegisterRepositoryServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");

app.ConfigureSwagger();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapContactEndpoints();

app.MapSchoolEndpoints();

app.MapMetadataEndpoints();

app.UseCors("CorsPolicy");

app.Run();
