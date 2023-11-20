using ToDo.Infra.Data.Util;
using AutoMapper;
using ToDo.Domain.Entities;
using ToDo.Domain.Contracts.Repository;
using ToDo.Application.DTO;
using ToDo.Application.Services;
using ToDo.Application.Interfaces;
using ToDo.Infra.Data.Repository;
using ToDo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

void AutoMapperDependencyInjection()
{
    var automapperconfigure = new MapperConfiguration(options =>
    { 
        options.CreateMap<User, UserDTO >().ReverseMap(); 
        options.CreateMap<Assignment, AssignmentDTO >().ReverseMap(); 
        options.CreateMap<AssignmentList, AssignmentListDTO >().ReverseMap(); 
    });

    builder.Services.AddSingleton(automapperconfigure.CreateMapper());
}

AutoMapperDependencyInjection();

var connectionstring = builder.Configuration.GetConnectionString("DEFAULT");

builder.Services.AddDbContext<ToDoContext>(options => options.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring)));

builder.Services.AddSingleton(d => builder.Configuration);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentListRepository, AssignmentListRepository>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
