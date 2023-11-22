using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.API.Controllers;
[ApiController]
[Route("/Authenticaton")]
public class AuthController : ControllerBase
{
    public AuthController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
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
    public async Task<IActionResult> Cadastro([FromForm]SingInUser user)
    {
        var usermapped = _mapper.Map<UserDTO>(user);
        var usercreated =  await _userService.CreateUser(usermapped);
        return Ok(usercreated);
    }
    
}