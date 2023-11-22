using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.API.Controllers;
[ApiController]
public class UserController : ControllerBase
{
    public UserController(IUserService userService, IAssignmentService assignmentService, IMapper mapper, IAssignmentListService assignmentListService, IAdminService adminService)
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
    public async Task<IActionResult> CreateTask([FromForm] AddAssignmentDTO assignmentDto)
    {
        
        var assignmentMapper = _mapper.Map<AssignmentDTO>(assignmentDto);
        assignmentMapper.UserId = AuthenticatedUser.Id;
        return Ok(await _assignmentService.CreateTask(assignmentMapper));

    }    
    
    [HttpPost]
    [Route("/CreateTaskList")]
    public async Task<IActionResult> CreateTaskList([FromForm] AddAssignmentListDTO assignmentDto)
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
    
    
}