namespace Modular.Web.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
            
        }

        public void Notify(string notification)
        {
            this.Clients.All.notify(notification);
        }
    }
}