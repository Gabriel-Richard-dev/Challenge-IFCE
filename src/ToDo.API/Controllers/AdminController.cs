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

    public AdminController(IAdminService adminService, IAssignmentService assignmentService, IAssignmentListService assignmentListService, IUserService userService)
    {
        _adminService = adminService;
        _assignmentListService = assignmentListService;
        _userService = userService;
        _assignmentService = assignmentService;
    }

    private readonly IAdminService _adminService;
    private readonly IAssignmentService _assignmentService;
    private readonly IAssignmentListService _assignmentListService;
    private readonly IUserService _userService;

    [HttpPost]
    [Route("/CriarUsuario")]
    public async Task<IActionResult> CreateUser([FromForm] UserDTO user)
    {
        await _userService.CreateUser(user);
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

    [HttpGet]
    [Route("/GetListsByUserId")]
    public async Task<IActionResult> GetTaskList(long userid)
    {
        var user = await _adminService.GetUserById(userid);
        if (user is not null)
        {
            return Ok(await _assignmentListService.GetAllLists(userid));
        }

        throw new Exception();
    }
    
    [HttpGet]
    [Route("/GetTasksByIds")]
    public async Task<IActionResult> GetTasksByIds(long userid, long listid)
    {
        return Ok(await _assignmentService.GetTasks(userid, listid));
    }
    
    [HttpDelete]
    [Route("/DeleteUser/{id}")]

    public async Task<IActionResult> DeleteUser(long id)
    {
        var userdeleted = await _adminService.GetUserById(id);
        await _adminService.RemoveUser(id);
        return Ok(userdeleted);
    }
    
    [HttpDelete]
    [Route("/DeleteTask/{id}")]

    public async Task<IActionResult> DeleteTask(long userid, long listid)
    {
        var task = await _assignmentService.GetTaskById();
        
        await _adminService.RemoveUser(id);
        return Ok(userdeleted);
    }
    
    
    
}