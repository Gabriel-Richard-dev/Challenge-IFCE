using System;
using AutoMapper;
using ToDo.Application.Interfaces;
using ToDo.Application.DTO;
using ToDo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Entities;
using ToDo.Core.ViewModel;



namespace ToDo.API.Controllers;

[ApiController]
public class AdminController : ControllerBase
{
    public AdminController(IAdminService adminService, IAssignmentService assignmentService,
        IAssignmentListService assignmentListService, IUserService userService, IMapper mapper)
    {
        _adminService = adminService;
        _assignmentListService = assignmentListService;
        _userService = userService;
        _mapper = mapper;
        _assignmentService = assignmentService;
    }

    private readonly IAdminService _adminService;
    private readonly IAssignmentService _assignmentService;
    private readonly IAssignmentListService _assignmentListService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    [HttpPost]
    [Route("/CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        await _userService.CreateUser(user);

        return Ok(new ResultViewModel
        {

            Message = "User created sucessfully.",
            Sucess = true,
            Data = user

        });
    }

    [HttpPost]
    [Route("/CreateListToUser")]
    public async Task<IActionResult> CreateList([FromBody] AssignmentListDTO list)
    {
        await _adminService.DelegateList(list);
        return Ok(new ResultViewModel
        {

            Message = "List created sucessufully.",
            Sucess = true,
            Data = list

        });
    }

    [HttpPost]
    [Route("/DelegateTask")]
    public async Task<IActionResult> DelegateTask([FromBody] AssignmentDTO assignment)
    {
        var assignmentCreated = await _assignmentService.CreateTask(assignment);
        return Ok(new ResultViewModel
        {
            Message = $"Task delegated to user {_adminService.GetUserById(assignment.UserId).Result.Name}",
            Sucess = true,
            Data = assignment
        });
    }

    [HttpGet]
    [Route("/GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(new ResultViewModel
        {
            Message = "List of Users:",
            Sucess = true,
            Data = await _userService.GetAllUsers()
        });
    }

    [HttpGet]
    [Route("/GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(long id)
    {

        var user = await _adminService.GetUserById(id);


        return Ok(new ResultViewModel
        {
            Message = "User recevied with sucessfully!",
            Sucess = true,
            Data = user
        });
    }

    [HttpGet]
    [Route("/GetListsByUserId")]
    public async Task<IActionResult> GetTaskList(long userid)
    {
        var user = await _adminService.GetUserById(userid);
        
        if (user is not null)
        {
            return Ok(new ResultViewModel
            {
                Message = $"List of taks of user: {user.Name}",
                Sucess = true,
                Data = await _assignmentListService.GetAllLists(userid)
            });
        }

        throw new Exception();
    }


    [HttpGet]
    [Route("/GetTaskByIds/{id}/{listId}/{userId}")]
    public async Task<IActionResult> GetTaskByIds(long id, long listId, long userId)
    {
        var search = new SearchAssignmentDTO()
        {
            Id = id,
            ListId = listId,
            UserId = userId
        };
        var task = await _assignmentService.GetTaskById(search);
        return Ok(new ResultViewModel
        {
            Message = $"Tasks do user {_adminService.GetUserById(userId).Result.Name}",
            Sucess = true,
            Data = task
        });
    }

    [HttpGet]
    [Route("/GetTasks/{userId}/{listId}")]
    public async Task<IActionResult> GetTasks(long userId, long listId)
    {
        return Ok(new ResultViewModel
        {
            Message = $"Tasks of {_adminService.GetUserById(userId).Result.Name}",
            Sucess = true,
            Data = await _assignmentService.GetTasks(userId, listId)

        });
    }

    [HttpGet]
    [Route("/GetByEmail/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        return Ok(new ResultViewModel
        {
            Message ="User recevied with sucess",
            Sucess = true,
            Data = await _userService.GetByEmail(email)
        });
    }

    [HttpDelete]
    [Route("/DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        var userdeleted = await _adminService.GetUserById(id);
        await _adminService.RemoveUser(id);
        return Ok(new ResultViewModel
        {
            Message = "User deleted with sucess",
            Sucess = true,
            Data = userdeleted
        });
    }

    [HttpDelete]
    [Route("/DeleteTask/")]
    public async Task<IActionResult> DeleteTask(SearchAssignmentDTO dto)
    {
        await _assignmentService.RemoveTask(dto);
        return Ok(new ResultViewModel
        {
            Message = "Task deleted with sucess",
            Sucess = true,
            Data = dto
        });
    }

    [HttpDelete]
    [Route("/DeleteTaskList")]
    public async Task<IActionResult> DeleteTaskList([FromBody] SearchAssignmentListDTO search)
    {
        await _assignmentListService.RemoveTaskList(search);
        return Ok(new ResultViewModel
        {
            Message = "TaskList deleted with success",
            Sucess = true,
            Data = search
        });
    }

    [HttpPut]
    [Route("UpdateUser/{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UserDTO usr, long id)
    {
        await _userService.Update(usr, id);
        return Ok(new ResultViewModel
        {
            Message = "User updated",
            Sucess = true,
            Data = usr
        });
    }
    
    
    [HttpPut]
    [Route("UpdateTask/{id}")]
    public async Task<IActionResult> UpdateTask([FromBody] AddAssignmentDTO dto, long id)
    {
        await _assignmentService.UpdateTask(dto, id);
        return Ok(new ResultViewModel
        {
            Message = "Task updated",
            Sucess = true,
            Data = dto
        });
    }

    [HttpGet]
    [Route("SearchUserByName/{parsename}")]
    public async Task<IActionResult> SearchByName(string parsename)
    {
        return Ok(new ResultViewModel
        {
            Message = "Users corresponding :",
            Sucess = true,
            Data = await _userService.SearchByName(parsename)
        });
    }
    
    
    [HttpGet]
    [Route("SearchUserByEmail/{parseEmail}")]
    public async Task<IActionResult> SearchByEmail(string parseEmail)
    {
        return Ok(new ResultViewModel
        {
            Message = "Users corresponding :",
            Sucess = true,
            Data = await _userService.SearchByEmail(parseEmail)
        });
    }

    
}