using System;
using ToDo.Application.Services;
using ToDo.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.API.Controllers;

[ApiController]
[Route("/Admin")]
public class AdminController : ControllerBase
{

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    private readonly AdminService _adminService;

    [HttpPost]
    [Route("/CriarUsuario")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        _adminService.CreateUser(user);
        return Ok(user);
    }


}