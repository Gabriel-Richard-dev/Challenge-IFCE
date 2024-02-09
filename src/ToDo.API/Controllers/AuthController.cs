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
    
    
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [HttpPost]
    [Route("/Login")]
    public async Task<IActionResult> Login([FromBody]LoginUserDTO dto)
    {
        string token = null;
        if (await _userService.LoginValid(dto))
        {
            var user = await _adminService.UserLogged(dto.Email);
            AuthenticatedUser.Id = user.Id;

            token = TokenService.GenerateToken(user);

        }


        return CustomResponse(token);


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
    
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPut]
    [Route("/RecoveryPassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] LoginUserDTO userDto, string newPassword)
    {
        await _userService.UpdatePassword(userDto, newPassword);
        return CustomResponse("Senha alterada com sucesso");
    }

}