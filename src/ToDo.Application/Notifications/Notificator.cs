namespace ToDo.Application.Notifications;

public class Notificator : INotification
{

    private List<string> _notifications = new();
    
    public void AddNotification(string message)
    {
        _notifications.Add(message);
    }

    public List<string> GetNotifications()
    {
        return _notifications;
    }

    public bool HasNotification() => _notifications.Any();


}