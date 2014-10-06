namespace Infrastructure.Notifications
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.AspNet.SignalR.Client.Hubs;

    public class NotificationSender : INotificationSender, IDisposable
    {
        private readonly HubConnection connection;
        private readonly IHubProxy proxy;

        public NotificationSender(Uri uri)
        {
            Contract.Requires(uri != null);

            this.connection = new HubConnection(uri.ToString());

            this.connection.Closed += this.OnClosed;
            this.connection.Error += this.OnError;
            this.connection.Received += this.OnReceived;
            this.connection.Reconnected += this.OnReconnected;

            this.proxy = this.connection.CreateHubProxy("NotificationHub");

            this.connection.Start();
        }

        public void Notify(string notification)
        {
            Contract.Requires(!string.IsNullOrEmpty(notification));

            this.proxy.Invoke("Notify", notification);
        }

        public void Dispose()
        {
            this.connection.Stop();
        }

        private void OnReconnected()
        {
        }

        private void OnReceived(string s)
        {
        }

        private void OnError(Exception error)
        {
        }

        private void OnClosed()
        {
        }
    }
}
