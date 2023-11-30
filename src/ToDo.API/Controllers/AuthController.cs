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
    public AuthController(IUserService userService, IMapper mapper, IAdminService adminService)
    {
        _userService = userService;
        _mapper = mapper;
        _adminService = adminService;
    }

    private readonly IUserService _userService;
    private readonly IAdminService _adminService;
    private readonly IMapper _mapper; 
    [HttpPost]
    [Route("/Login")]
    public async Task<IActionResult> Login([FromBody]LoginUserDTO dto)
    {
        var listUsers = await _userService.GetAllUsers();
        
        if (await _userService.LoginValid(dto))
        {
            var user = await _userService.GetByEmail(dto.Email);
            AuthenticatedUser.Id = user.Id;
            return Ok("Logado com sucesso!");
        }

        throw new Exception();

        
    }
    
    [HttpPost]
    [Route("/Cadastro")]
    public async Task<IActionResult> Cadastro([FromBody]SingInUserDTO userDto)
    {
        var usermapped = _mapper.Map<UserDTO>(userDto);
        var usercreated =  await _userService.CreateUser(usermapped);
        return Ok(usercreated);
    }
    
    [HttpPut]
    [Route("/RecoveryPassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] LoginUserDTO userDto)
    {
        var parseuser = _adminService.GetCredentials(userDto.Email).Result;
        var senha = parseuser.Password;
        var search = new LoginUserDTO()
        {
            Email = userDto.Email,
            Password = senha
        };
        await _userService.UpdatePassword(search, search.Password, userDto.Password);
        return Ok("Senha alterada com sucesso");
    }

}