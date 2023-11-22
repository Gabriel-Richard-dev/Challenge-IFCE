using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.API.Controllers;
[ApiController]
[Route("/Authenticaton")]
public class AuthController : ControllerBase
{
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    private readonly IUserService _userService;
    
    [HttpPost]
    [Route("/Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var listUsers = await _userService.GetAllUsers();
        var userExists = listUsers.Where(u => u.Email == email && u.Password == password).ToList();
        if (userExists.FirstOrDefault() is not null)
        {
            AuthenticatedUser.Id = userExists.FirstOrDefault().Id;
            return Ok("Logado com sucesso!");
        }

        throw new Exception();

        
    }

    [HttpPost]
    [Route("/Cadastre-se")]
    public async Task<IActionResult> Cadastro(string name, string email, string password)
    {
        UserDTO user = new UserDTO()
        {
            Name = name,
            Email = email,
            Password = password,
            AdminPrivileges = false
        };
        var usercreated =  await _userService.CreateUser(user);
        return Ok(usercreated);
    }
    
}