using System;
using ToDo.Application.Interfaces;
using ToDo.Application.DTO;
using ToDo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Entities;


namespace ToDo.API.Controllers;

[ApiController]
public class AdminController : ControllerBase
{

    public AdminController(IAdminService adminService, IAssignmentService assignmentService)
    {
        _adminService = adminService;
        _assignmentService = assignmentService;
    }

    private readonly IAdminService _adminService;
    private readonly IAssignmentService _assignmentService;

    [HttpPost]
    [Route("/CriarUsuario")]
    public async Task<IActionResult> CreateUser([FromForm] UserDTO user)
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
    public async Task<IActionResult> DelegateTask([FromForm] AssignmentDTO assignment)
    {
        var assignmentCreated = await _assignmentService.CreateTask(assignment);
        return Ok(assignment);
    }

    [HttpGet]
    [Route("/GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _adminService.GetAllUsers());
    }
    
    [HttpGet]
    [Route("/GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        return Ok(await _adminService.GetUserById(id));
    }
    
    
    


}