using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.API.Controllers;
[ApiController]
public class UserController : ControllerBase
{
    public UserController(IUserService userService, IAssignmentService assignmentService, IMapper mapper)
    {
        _userService = userService;
        _assignmentService = assignmentService;
        _mapper = mapper;
    }

    private readonly IUserService _userService;
    private readonly IAssignmentService _assignmentService;
    private readonly IMapper _mapper;

    [HttpPost]
    [Route("/CreateTask")]
    public async Task<IActionResult> CreateTask([FromForm] AssignmentDTO assignmentDto)
    {
        var assigmentCreated = await _assignmentService.CreateTask(assignmentDto);
        return Ok(assignmentDto);
    }
    
    
    
    
    
    
}