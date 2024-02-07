using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Application.Notifications;
using ToDo.Application.Services;
using ToDo.Core.ViewModel;
using ToDo.Domain.Entities;

namespace ToDo.API.Controllers;

[ApiController]
[Route("/Authenticaton")]
public class AuthController : BaseController
{
    public AuthController(IUserService userService, IMapper mapper, IAdminService adminService, IAssignmentListService listservice, 
        INotification notification) : base(notification)
    {
        _userService = userService;
        _mapper = mapper;
        _adminService = adminService;
        _assignmentListService = listservice;
    }

    private readonly IUserService _userService;
    private readonly IAdminService _adminService;
    private readonly IAssignmentListService _assignmentListService;
    private readonly IMapper _mapper;
    
    
    
    [HttpPost]
    [Route("/Login")]
    public async Task<IActionResult> Login([FromBody]LoginUserDTO dto)
    {

        if (await _userService.LoginValid(dto))
        {
            var user = await _adminService.UserLogged(dto.Email);
            AuthenticatedUser.Id = user.Id;

            var token = TokenService.GenerateToken(user);
            return Ok(new ResultViewModel
            {
                Message = $"Token created sucessfully:",
                Sucess = true,
                Data = token
            });
        }

        throw new Exception();

        
    }
    
    [HttpPost]
    [Route("/Cadastro")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cadastro([FromBody]SingInUserDTO userDto)
    {
        var usermapped = _mapper.Map<UserDTO>(userDto);
        
        return CustomResponse(await _userService.CreateUser(usermapped));
        
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