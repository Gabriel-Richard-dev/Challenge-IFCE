using System;
using AutoMapper;
using ToDo.Application.Interfaces;
using ToDo.Application.DTO;
using ToDo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Entities;
using ToDo.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using ToDo.Application.Notifications;


namespace ToDo.API.Controllers;

[ApiController]
public class AdminController : BaseController
{
    public AdminController(IAdminService adminService, IAssignmentService assignmentService,
        IAssignmentListService assignmentListService, IUserService userService, IMapper mapper, INotification notification) : base(notification)
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

    [Authorize(Roles = "True")]
    [HttpPost]
    [Route("/CreateUser")]
    
    
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser(UserDTO user)
    {
        return CustomResponse(await _userService.CreateUser(user));
    }
    [Authorize(Roles = "True")]
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
    [Authorize(Roles = "True")]
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

    [Authorize(Roles = "True")]
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
    
    [Authorize(Roles = "True")]
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
    [Authorize(Roles = "True")]
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

    [Authorize(Roles = "True")]
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
    [Authorize(Roles = "True")]
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
    [Authorize(Roles = "True")]
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
    
    [Authorize(Roles = "True")]
    [HttpDelete]
    [Route("/DeleteUser/{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(long id)
    {
        return CustomResponse(_adminService.RemoveUser(id));
    }
    
    
    [Authorize(Roles = "True")]
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
    [Authorize(Roles = "True")]
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
    [Authorize(Roles = "True")]
    [HttpPut]
    [Route("UpdateUser/{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser([FromBody] UserDTO usr, long id)
    {
       return CustomResponse(await _userService.Update(usr, id));
    }

    [Authorize(Roles = "True")]
    [HttpPut]
    [Route("UpdateTask/{id}")]
    public async Task<IActionResult> UpdateTask([FromBody] AddAssignmentDTO dto, long id)
    {
        return CustomResponse(await _assignmentService.UpdateTask(dto, id));
    }
    [Authorize(Roles = "True")]
    [HttpGet]
    [Route("SearchUserByName/{parsename}")]
    public async Task<IActionResult> SearchByName(string parsename)
    {
        return Ok(new ResultViewModel
        {
            Message = $"Users corresponding to '{parsename}':",
            Sucess = true,
            Data = await _userService.SearchByName(parsename)
        });
    }

    [Authorize(Roles = "True")]
    [HttpGet]
    [Route("SearchUserByEmail/{parseEmail}")]
    public async Task<IActionResult> SearchByEmail(string parseEmail)
    {
        return Ok(new ResultViewModel
        {
            Message = $"Users corresponding to '{parseEmail}':",
            Sucess = true,
            Data = await _userService.SearchByEmail(parseEmail)
        });
    }

    
}