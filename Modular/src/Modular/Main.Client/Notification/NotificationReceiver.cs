namespace Main.Notification
{
    using System;
    using System.Diagnostics.Contracts;
    using Infrastructure.Events;
    using SignalR.Client.Hubs;

    public class NotificationReceiver : IDisposable
    {
        private readonly IEventAggregator eventAggregator;
        private readonly HubConnection connection;
        private readonly IDisposable subscription;
        private readonly IHubProxy proxy;

        public NotificationReceiver(IEventAggregator eventAggregator, Uri uri)
        {
            Contract.Requires(eventAggregator != null);
            Contract.Requires(uri != null);

            this.eventAggregator = eventAggregator;

            this.connection = new HubConnection(uri.ToString());

            this.connection.Closed += this.OnClosed;
            this.connection.Error += this.OnError;
            this.connection.Received += this.OnReceived;
            this.connection.Reconnected += this.OnReconnected;

            this.proxy = this.connection.CreateProxy("NotificationHub");

            this.subscription = this.proxy.On("notify", (string notification) => this.OnNotified(notification));

            this.connection.Start();
        }

        public void Dispose()
        {
            this.connection.Stop();
            this.subscription.Dispose();
        }

        private void OnNotified(string notification)
        {   
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
