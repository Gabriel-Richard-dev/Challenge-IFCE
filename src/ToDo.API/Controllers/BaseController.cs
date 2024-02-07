using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Notifications;

namespace ToDo.API.Controllers;


[ApiController]
public abstract class BaseController : ControllerBase
{
    private readonly INotification _notification;
    
    protected BaseController(INotification notification)
    {
        _notification = notification;
    }

    protected ActionResult CustomResponse(object? result = null)
    {
        if (ValidOperation)
        {
            return Ok(result);
        }


        if (_notification.notFound)
        {
            return NotFound();
        }

        return BadRequest
        (
            error: _notification.GetNotifications().ToList()
        );
        
    }
    
    
    
    
    
    


    private bool ValidOperation => !(_notification.HasNotification() || _notification.notFound);
}