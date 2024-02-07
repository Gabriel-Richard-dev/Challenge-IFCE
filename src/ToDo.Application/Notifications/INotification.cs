using Microsoft.IdentityModel.Tokens;

namespace ToDo.Application.Notifications;

public interface INotification
{
    bool notFound { get; set; }
    void AddNotification(string message);

    void AddNotification(List<string> messages);
    
    List<string> GetNotifications();
    
    bool HasNotification();

    void NotFound();
}