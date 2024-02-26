using System;
using ToDo.Application.Interfaces;
using ToDo.Application.DTO;
using ToDo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Entities;
using ToDo.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using ToDo.Application.Notifications;


namespace ToDo.API.Controllers;

[ApiController]
public class AdminController : BaseController
{
    public AdminController(IAdminService adminService, IAssignmentService assignmentService,
        IAssignmentListService assignmentListService, IUserService userService,
        INotification notification) : base(notification)
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
    
    [Authorize(Roles = "True")]
    [HttpPost]
    [Route("/CreateUser")]
    [SwaggerOperation(Summary = "Create an user")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser(UserDTO user)
    {
        return CustomResponse(await _userService.CreateUser(user));
    }
    
    
    [SwaggerOperation(Summary = "Create a List")]
    [Authorize(Roles = "True")]
    [HttpPost]
    [Route("/CreateListToUser")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(AssignmentList), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateList([FromBody] AssignmentListDTO list)
    {
        return CustomResponse(await _adminService.DelegateList(list));
    }
    
    [SwaggerOperation(Summary = "Delegate a Task")]
    [Authorize(Roles = "True")]
    [HttpPost]
    [Route("/DelegateTask")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Assignment), StatusCodes.Status200OK)]
    public async Task<IActionResult> DelegateTask([FromBody] AssignmentDTO assignment)
    {
        return CustomResponse(await _assignmentService.CreateTask(assignment));
    }

    [SwaggerOperation(Summary = "Get all users")]
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
    
    [SwaggerOperation(Summary = "GetUser by Id")]
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
    
    [SwaggerOperation(Summary = "Get tasks list")]
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
    [SwaggerOperation(Summary = "Get user task by ids (TASKID,LISTID,USERID)")]
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
    
    [SwaggerOperation(Summary = "Get all tasks")]
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
    
    [SwaggerOperation(Summary = "Get user by email")]
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
    
    [SwaggerOperation(Summary = "Delete an user")]
    [Authorize(Roles = "True")]
    [HttpDelete]
    [Route("/DeleteUser/{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(long id)
    {
        return CustomResponse(await _adminService.RemoveUser(id));
    }
    
    [SwaggerOperation(Summary = "Delete a task")]
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
    [SwaggerOperation(Summary = "Delegate tasklist user")]
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
    
    
    [SwaggerOperation(Summary = "Login, enter your credentials")]
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
    
    [SwaggerOperation(Summary = "Update your task")]
    [Authorize(Roles = "True")]
    [HttpPut]
    [Route("UpdateTask/{id}")]
    public async Task<IActionResult> UpdateTask([FromBody] AddAssignmentDTO dto, long id)
    {
        return CustomResponse(await _assignmentService.UpdateTask(dto, id));
    }
    
    [SwaggerOperation(Summary = "Search an user by name")]
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
    
    [SwaggerOperation(Summary = "Search an user by email")]
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