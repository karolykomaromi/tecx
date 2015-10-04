namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Linq;
    using System.Reactive;
    using Hydra.Cooling.Sensors;

    public class CompositeAlert<TEventArgs> : IAlert<TEventArgs>
        where TEventArgs : SensorStateChangedEventArgs
    {
        private readonly IAlert<TEventArgs>[] alerts;

        public CompositeAlert(params IAlert<TEventArgs>[] alerts)
        {
            this.alerts = alerts ?? new IAlert<TEventArgs>[0];
        }

        public IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            IDisposable[] subscriptions = this.alerts.Select(alert => alert.Subscribe(observer)).ToArray();

            return new CompositeSubscription(subscriptions);
        }

        private class CompositeSubscription : IDisposable
        {
            private readonly IDisposable[] subscriptions;

            public CompositeSubscription(params IDisposable[] subscriptions)
            {
                this.subscriptions = subscriptions ?? new IDisposable[0];
            }

            public void Dispose()
            {
                foreach (IDisposable subscription in this.subscriptions)
                {
                    subscription.Dispose();
                }
            }
        }
    }
}