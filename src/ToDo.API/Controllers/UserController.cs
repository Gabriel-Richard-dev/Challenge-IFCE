using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Core.ViewModel;

namespace ToDo.API.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    public UserController(IUserService userService, IAssignmentService assignmentService, IMapper mapper,
        IAssignmentListService assignmentListService, IAdminService adminService)
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

    [HttpPost]
    [Route("/CreateTask")]
    public async Task<IActionResult> CreateTask([FromBody] AddAssignmentDTO assignmentDto)
    {
        var assignmentMapper = _mapper.Map<AssignmentDTO>(assignmentDto);
        assignmentMapper.UserId = AuthenticatedUser.Id;
        var taskCreated = await _assignmentService.CreateTask(assignmentMapper);
        return Ok(new ResultViewModel
        {

        });
    }

    [HttpPost]
    [Route("/CreateTaskList")]
    public async Task<IActionResult> CreateTaskList([FromBody] AddAssignmentListDTO assignmentDto)
    {
        var assignmentMapper = _mapper.Map<AssignmentListDTO>(assignmentDto);
        assignmentMapper.UserId = AuthenticatedUser.Id;
        return Ok(await _assignmentListService.CreateList(assignmentMapper));
    }


    [HttpGet]
    [Route("/GetLists")]
    public async Task<IActionResult> GetTaskList()
    {
        var userid = AuthenticatedUser.Id;
        var user = await _adminService.GetUserById(userid);
        if (user is not null)
        {
            return Ok(await _assignmentListService.GetAllLists(userid));
        }

        throw new Exception();
    }

    [HttpGet]
    [Route("/GetUserTasksByIds/{listid}")]
    public async Task<IActionResult> GetTasksByIds(long listid)
    {
        var userId = AuthenticatedUser.Id;
        return Ok(await _assignmentService.GetTasks(userId, listid));
    }

    [HttpPost]
    [Route("/GetUserTask")]
    public async Task<IActionResult> GetTask([FromBody] UserSearchAssignmentDTO dto)
    {
        var search = _mapper.Map<SearchAssignmentDTO>(dto);
        search.UserId = dto.UserId;
        var assignment = await _assignmentService.GetTaskById(search);

        return Ok(assignment);
    }

    [HttpDelete]
    [Route("/DeleteUserTask/{id}")]
    public async Task<IActionResult> DeleteTask(long id, long listId)
    {
        var search = new SearchAssignmentDTO() { Id = id, ListId = listId, UserId = AuthenticatedUser.Id };

        return Ok(await _assignmentService.RemoveTask(search));
    }

    [HttpPut]
    [Route("/UpdatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] LoginUserDTO search, string confirmpassword,
         string newpassword)
    {
        return Ok(await _userService.UpdatePassword(search, confirmpassword, newpassword));
    }

    [HttpPut]
    [Route("/UpdateUserTask/{id}")]
    public async Task<IActionResult> UpdateUserTask([FromBody] AddAssignmentDTO dto, long id)
    {
        return Ok(await _assignmentService.UpdateUserTask(dto, id));
    }


    [HttpGet]
    [Route("/SearchTaskByTitle/{parseTitle}")]
    public async Task<IActionResult> SearchTaskByTitle(string parseTitle)
    {
        return Ok(await _assignmentService.SearchTaskByTitle(parseTitle));
    }
}