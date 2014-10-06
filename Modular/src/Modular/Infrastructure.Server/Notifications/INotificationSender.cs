namespace Infrastructure.Notifications
{
    public interface INotificationSender
    {
        void Notify(string notification);
    }
}