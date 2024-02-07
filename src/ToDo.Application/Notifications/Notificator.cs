namespace ToDo.Application.Notifications;

public class Notificator : INotification
{

    private List<string> _notifications = new();

    public bool notFound { get; set; } = false;

    public void AddNotification(string message)
    {
        _notifications.Add(message);
    }

    public void AddNotification(List<string> messages)
    {
        _notifications.AddRange(messages);
    }

    public List<string> GetNotifications()
    {
        return _notifications;
    }

    public bool HasNotification() => _notifications.Any();
    public void NotFound()
    {
        notFound = true;
    }
}