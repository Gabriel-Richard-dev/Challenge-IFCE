using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Util;

public static class DependencyInjection 
{
    public static void AddInfraData(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ToDoContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }

}

