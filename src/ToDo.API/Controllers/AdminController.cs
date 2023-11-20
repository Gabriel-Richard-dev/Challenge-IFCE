using System;
using ToDo.Application.Interfaces;
using ToDo.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.API.Controllers;

[ApiController]
[Route("/Admin")]
public class AdminController : ControllerBase
{

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    private readonly IAdminService _adminService;

    [HttpPost]
    [Route("/CriarUsuario")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        _adminService.CreateUser(user);
        return Ok(user);
    }


}