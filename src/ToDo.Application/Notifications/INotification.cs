using Microsoft.IdentityModel.Tokens;

namespace ToDo.Application.Notifications;

public interface INotification
{
    void AddNotification(string message);
    List<string> GetNotifications();
    bool HasNotification();

}