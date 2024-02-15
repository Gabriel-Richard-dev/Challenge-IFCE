using AutoMapper;
using Microsoft.AspNetCore.Hosting.Builder;
using ToDo.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using ToDo.Domain.Keys;
using ToDo.Application.DTO;
using ToDo.Application.Services;
using ToDo.Application.Interfaces;
using ToDo.Infra.Data.Repository;
using ToDo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using System.Text;
using Microsoft.OpenApi.Models;
using ToDo.API.Configuration;
using ToDo.Application.Configuration;
using ToDo.Application.Notifications;
using ToDo.Infra.Data.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton(d => builder.Configuration);

//configuration
builder.Services.ConfigureCors();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAplicattionConfig(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.AuthenticationConfiguration(builder.Configuration);
//endprogrammerconfigs

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();