using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Configuration;

public static class DependencyInjection
{
    public static void AddAplicattionConfig(this IServiceCollection services, IConfiguration configuration)
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

        services.AddSingleton(automapperconfigure.CreateMapper());
    }
        
}
