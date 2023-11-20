using System;
using ToDo.Application.Interfaces;
using ToDo.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Entities;

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
        await _adminService.CreateUser(user);
        return Ok(user);
    }
    [HttpPost]
    [Route("/CreateListToUser")]
    public async Task<IActionResult> CreateList([FromBody] AssignmentListDTO list)
    {
        await _adminService.DelegateList(list);
        return Ok(list);
    }
    [HttpPost]
    [Route("/DelegateTask")]
    public async Task<IActionResult> DelegateTask([FromBody] AssignmentDTO assignment)
    {
        await _adminService.DelegateTask(assignment);
        return Ok(assignment);
    }
    
    
    


}