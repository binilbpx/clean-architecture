using CleanArchitecture.API.StartUp;
using CleanArchitecture.API.Validation;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using CleanArchitecture.Infrastructure.Repository;
using FluentValidation;
using FluentValidation.Results;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.RegisterServices();

var app = builder.Build();

app.ConfigureSwagger();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapContactEndpoints();

app.Run();
