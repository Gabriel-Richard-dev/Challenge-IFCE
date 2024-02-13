using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Core.ViewModel;
using ToDo.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using ToDo.Application.Notifications;

namespace ToDo.API.Controllers;

[ApiController]
public class UserController : BaseController
{
    public UserController(IUserService userService, IAssignmentService assignmentService, IMapper mapper,
        IAssignmentListService assignmentListService, IAdminService adminService, INotification notification) : base(notification)
    {
        _userService = userService;
        _assignmentService = assignmentService;
        _mapper = mapper;
        _assignmentListService = assignmentListService;
        _adminService = adminService;
    }

    private readonly IUserService _userService;
    private readonly IAdminService _adminService;
    private readonly IAssignmentService _assignmentService;
    private readonly IAssignmentListService _assignmentListService;
    private readonly IMapper _mapper;
    
    [Authorize]
    [HttpPost]
    [Route("/CreateTask")]
    [ProducesResponseType(typeof(Assignment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateTask([FromBody] AddAssignmentDTO assignmentDto)
    {
        var assignmentMapper = _mapper.Map<AssignmentDTO>(assignmentDto);
        assignmentMapper.UserId = AuthenticatedUser.Id;
        var taskCreated = await _assignmentService.CreateTask(assignmentMapper);
        return CustomResponse(taskCreated);
    }
    [Authorize]
    [HttpPost]
    [Route("/CreateTaskList")]
    public async Task<IActionResult> CreateTaskList([FromBody] AddAssignmentListDTO assignmentDto)
    {
        var assignmentMapper = _mapper.Map<AssignmentListDTO>(assignmentDto);
        assignmentMapper.UserId = AuthenticatedUser.Id;
        var taskListCreated = await _assignmentListService.CreateList(assignmentMapper);
        return Ok(new ResultViewModel()
        {
            Message = "TaskList Created",
            Sucess = true,
            Data = assignmentDto
        });
    }
    [Authorize]
    [HttpGet]
    [Route("/GetLists")]
    public async Task<IActionResult> GetTaskList()
    {
        var userid = AuthenticatedUser.Id;
        var user = await _adminService.GetUserById(userid);
        if (user is not null)
        {
            var lists = await _assignmentListService.GetAllLists(userid);
            return Ok(new ResultViewModel()
            {
                
                Message = $"Task list of user: {user.Name}",
                Sucess = true,
                Data = lists
            });
        }

        return BadRequest(new ResultViewModel()
        {
            Message = "User don't have lists",
            Sucess = false,
            Data = null
        });
    }
    [Authorize]
    [HttpGet]
    [Route("/GetUserTasksByIds/{listid}")]
    public async Task<IActionResult> GetTasksByIds(long listid)
    {
        var userId = AuthenticatedUser.Id;
        var tasks = await _assignmentService.GetTasks(userId, listid);
        return Ok(new ResultViewModel()
        {
            Message = $"Tasks recevied sucessfully of list ({listid})",
            Sucess = true,
            Data = tasks
        });
    }
    [Authorize]
    [HttpPost]
    [Route("/GetUserTask")]
    public async Task<IActionResult> GetTask([FromBody] UserSearchAssignmentDTO dto)
    {
        var search = _mapper.Map<SearchAssignmentDTO>(dto);
        search.UserId = dto.UserId;
        var assignment = await _assignmentService.GetTaskById(search);

        return Ok(assignment);
    }
    [Authorize]
    [HttpDelete]
    [Route("/DeleteUserTask/{id}")]
    public async Task<IActionResult> DeleteTask(long id, long listId)
    {
        var search = new SearchAssignmentDTO() { Id = id, ListId = listId, UserId = AuthenticatedUser.Id };
        await _assignmentService.RemoveTask(search);
        return Ok(new ResultViewModel() { 
            Message = "Task removed sucessfully",
            Sucess= true,
            Data = null
        });
    }
    [Authorize]
    [HttpPut]
    [Route("/UpdatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] LoginUserDTO search, string newpassword)
    {
        await _userService.UpdatePassword(search, newpassword);
        return CustomResponse();
    }
    [Authorize]
    [HttpPut]
    [Route("/UpdateUserTask/{id}")]
    public async Task<IActionResult> UpdateUserTask([FromBody] AddAssignmentDTO dto, long id)
    {
        await _assignmentService.UpdateUserTask(dto, id);
        return Ok(new ResultViewModel()
        {
            Message = "User updated sucessfully.",
            Sucess= true,
            Data = null
        });
    }

    [Authorize]
    [Authorize]
    [HttpGet]
    [Route("/SearchTaskByTitle/{parseTitle}")]
    public async Task<IActionResult> SearchTaskByTitle(string parseTitle)
    {
        var result = await _assignmentService.SearchTaskByTitle(parseTitle);
        return Ok(new ResultViewModel()
        {
            Message = $"You search:'{parseTitle}'\n Corresponding result:",
            Sucess = true,
            Data = result
        });
    }


   
}