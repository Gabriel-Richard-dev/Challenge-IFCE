using AutoMapper;
using Microsoft.AspNetCore.Hosting.Builder;
using ToDo.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using ToDo.Domain.Contracts.Repository;
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
using ToDo.Application.Notifications;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



void AutoMapperDependencyInjection()
{
    var automapperconfigure = new MapperConfiguration(options =>
    {
        options.CreateMap<User, UserDTO>().ReverseMap();
        options.CreateMap<SingInUserDTO, UserDTO>().ReverseMap();
        options.CreateMap<SearchUserDTO, User>().ReverseMap();
        options.CreateMap<Assignment, AssignmentDTO>().ReverseMap();
        options.CreateMap<AssignmentDTO, AddAssignmentDTO>().ReverseMap();
        options.CreateMap<AssignmentListDTO, AddAssignmentListDTO>().ReverseMap();
        options.CreateMap<AssignmentList, AssignmentListDTO>().ReverseMap();
        options.CreateMap<SearchAssignmentDTO, UserSearchAssignmentDTO>().ReverseMap();
        options.CreateMap<SearchAssignmentDTO, Assignment>().ReverseMap();
        options.CreateMap<AddAssignmentDTO, Assignment>().ReverseMap();
        options.CreateMap<UserDTO, SearchUserDTO>().ReverseMap();
        options.CreateMap<UserDTO, BaseUserDTO>().ReverseMap();
    });

    builder.Services.AddSingleton(automapperconfigure.CreateMapper());
}

AutoMapperDependencyInjection();

var connectionstring = builder.Configuration.GetConnectionString("DEFAULT");

builder.Services.AddDbContext<ToDoContext>(options => 
    options
        .UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring))
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging());

builder.Services.AddSingleton(d => builder.Configuration);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IAssignmentListService, AssignmentListService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentListRepository, AssignmentListRepository>();
builder.Services.AddScoped<INotification, Notificator>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new List<string>()
        }
    });
});


var key = Encoding.ASCII.GetBytes(ToDo.Domain.Keys.Key.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});





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