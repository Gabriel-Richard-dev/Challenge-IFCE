using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.API.Controllers;
[ApiController]
public class Authentication : ControllerBase
{
    public Authentication(IUserService userService)
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
            return Ok("Logado com sucesso!");
        }

        throw new Exception();


    }
    
}