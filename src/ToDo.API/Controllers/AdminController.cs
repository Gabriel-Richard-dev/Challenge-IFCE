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

            Message = "Usuário criado com sucesso!",
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

            Message = "Lista criada com sucesso!",
            Sucess = true,
            Data = list

        });
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
        return Ok(new ResultViewModel
        {
            Message = "Lista de Usuários:",
            Sucess = true,
            Data = await _userService.GetAllUsers()
        });
    }

    [HttpGet]
    [Route("/GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {

        var user = await _adminService.GetUserById(id);


        return Ok(new ResultViewModel
        {
            Message = "Usuário recebido com sucesso!",
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
                Message = $"Listas de tarefas do Usuário: {user.Name}",
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
            Message = "Usuário criado com sucesso:",
            Sucess = true,
            Data = task
        });
    }

    [HttpGet]
    [Route("/GetTasks/{userId}/{listId}")]
    public async Task<IActionResult> GetTasks(long userId, long listId)
    {
        return Ok(await _assignmentService.GetTasks(userId, listId));
    }

    [HttpGet]
    [Route("/GetByEmail/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        return Ok(await _userService.GetByEmail(email));
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
    [Route("/DeleteTask/")]
    public async Task<IActionResult> DeleteTask(SearchAssignmentDTO dto)
    {
      
        return Ok(await _assignmentService.RemoveTask(dto));
    }

    [HttpDelete]
    [Route("/DeleteTaskList")]
    public async Task<IActionResult> DeleteTaskList([FromForm] SearchAssignmentListDTO search)
    {
       
        return Ok(await _assignmentListService.RemoveTaskList(search));
    }

    [HttpPut]
    [Route("UpdateUser/{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UserDTO usr, long id)
    {
        return Ok(await _userService.Update(usr, id));
    }
    
    
    [HttpPut]
    [Route("UpdateTask/{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] AddAssignmentDTO dto, long id)
    {
        return Ok(await _assignmentService.UpdateTask(dto, id));
    }

    [HttpGet]
    [Route("SearchUserByName/{parsename}")]
    public async Task<IActionResult> SearchByName(string parsename)
    {
        return Ok(await _userService.SearchByName(parsename));
    }
    
    
    [HttpGet]
    [Route("SearchUserByEmail/{parseEmail}")]
    public async Task<IActionResult> SearchByEmail(string parseEmail)
    {
        return Ok(await _userService.SearchByEmail(parseEmail));
    }

    
}