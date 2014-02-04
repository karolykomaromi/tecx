namespace Modular.Web.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class NotificationHub : Hub
    {
        public void Notify(string notification)
        {
            this.Clients.All.notify(notification);
        }
    }
}