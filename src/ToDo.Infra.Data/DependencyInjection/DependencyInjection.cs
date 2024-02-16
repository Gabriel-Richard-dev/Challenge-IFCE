using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Interfaces;
using ToDo.Application.Notifications;
using ToDo.Application.Services;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;
using ToDo.Infra.Data.Repository;

namespace ToDo.Infra.Data.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionstring = configuration.GetConnectionString("DEFAULT");

        services.AddDbContext<ToDoContext>(options => 
            options
                .UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());
        
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IAssignmentListService, AssignmentListService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentListRepository, AssignmentListRepository>();
        services.AddScoped<INotification, Notificator>();

    }


}