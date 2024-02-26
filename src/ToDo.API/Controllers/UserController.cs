using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Core.ViewModel;
using ToDo.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
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
    [SwaggerOperation(Summary = "Create a task")]
    [Route("/CreateTask")]
    [ProducesResponseType(typeof(Assignment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateTask([FromBody] AddAssignmentDTO assignmentDto)
    {
        var assignment = _mapper.Map<AssignmentDTO>(assignmentDto);
        assignment.UserId = AuthenticatedUser.Id;
        return CustomResponse(await _assignmentService.CreateTask(assignment));
    }
    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a TaskList")]
    [Route("/CreateTaskList")]
    public async Task<IActionResult> CreateTaskList([FromBody] AddAssignmentListDTO assignmentDto)
    {
        var assignmentlist = _mapper.Map<AssignmentListDTO>(assignmentDto);
        assignmentlist.UserId = AuthenticatedUser.Id;
        return CustomResponse(await _adminService.DelegateList(assignmentlist));
    }
    [Authorize]
    [HttpGet]
    [SwaggerOperation(Summary = "Get a tasklist")]
    [Route("/GetLists")]
    public async Task<IActionResult> GetTaskList()
    {
        var userid = AuthenticatedUser.Id;
     
            var lists = await _assignmentListService.GetAllLists(userid);
            return CustomResponse(lists);
        
        
    }
    
    [Authorize]
    [HttpGet]
    [SwaggerOperation(Summary = "Get User Task by ListId")]
    [Route("/GetUserTasksByIds/{listid}")]
    public async Task<IActionResult> GetTasksByIds(long listid)
    {
        var userId = AuthenticatedUser.Id;
        var tasks = await _assignmentService.GetTasks(userId, listid);
        return CustomResponse(tasks);
    }
    
    
    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Get a user task")]
    [Route("/GetUserTask")]
    public async Task<IActionResult> GetTask([FromBody] UserSearchAssignmentDTO dto)
    {
        var search = _mapper.Map<SearchAssignmentDTO>(dto);
        search.UserId = dto.UserId;
        var assignment = await _assignmentService.GetTaskById(search);

        return CustomResponse(assignment);
    }
    [Authorize]
    [HttpDelete]
    [SwaggerOperation(Summary = "Delete a task")]
    [Route("/DeleteUserTask/{id}")]
    public async Task<IActionResult> DeleteTask(long id, long listId)
    {
        var search = new SearchAssignmentDTO() { Id = id, ListId = listId, UserId = AuthenticatedUser.Id };
        await _assignmentService.RemoveTask(search);
        return CustomResponse("Task removed");
    }
    
    [SwaggerOperation(Summary = "Update your Password")]
    [Authorize]
    [HttpPut]
    [Route("/UpdatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] LoginUserDTO search, string newpassword)
    {
        await _userService.UpdatePassword(search, newpassword);
        return CustomResponse();
    }
    [SwaggerOperation(Summary = "Update a user task")]
    [Authorize]
    [HttpPut]
    [Route("/UpdateUserTask/{id}")]
    public async Task<IActionResult> UpdateUserTask([FromBody] AddAssignmentDTO dto, long id)
    {
        await _assignmentService.UpdateUserTask(dto, id);
        return CustomResponse();
    }
    [SwaggerOperation(Summary = "search task by title")]
    [Authorize]
    [Authorize]
    [HttpGet]
    [Route("/SearchTaskByTitle/{parseTitle}")]
    public async Task<IActionResult> SearchTaskByTitle(string parseTitle)
    {
        var result = await _assignmentService.SearchTaskByTitle(parseTitle);
        return CustomResponse(result);
    }
}