namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive;
    using Hydra.Cooling.Sensors;

    [DebuggerDisplay("Count={Count}")]
    public class CompositeAlert<TEventArgs> : IAlert<TEventArgs>
        where TEventArgs : SensorStateChangedEventArgs
    {
        private readonly HashSet<IAlert<TEventArgs>> alerts;

        public CompositeAlert(params IAlert<TEventArgs>[] alerts)
        {
            this.alerts = new HashSet<IAlert<TEventArgs>>();

            this.AddRange(alerts);
        }

        public int Count
        {
            get { return this.alerts.Count; }
        }

        public IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            IDisposable[] subscriptions = this.alerts.Select(alert => alert.Subscribe(observer)).ToArray();

            return new CompositeSubscription(subscriptions);
        }

        private void AddRange(IEnumerable<IAlert<TEventArgs>> alerts)
        {
            if (alerts == null)
            {
                return;
            }

            foreach (var alert in alerts)
            {
                if (object.ReferenceEquals(this, alert))
                {
                    continue;
                }

                CompositeAlert<TEventArgs> other = alert as CompositeAlert<TEventArgs>;

                if (other != null)
                {
                    this.AddRange(other.alerts);
                }
                else
                {
                    this.alerts.Add(alert);
                }
            }
        }

        [DebuggerDisplay("Count={Count}")]
        private class CompositeSubscription : IDisposable
        {
            private readonly HashSet<IDisposable> subscriptions;

            private bool isDisposed;

            public CompositeSubscription(params IDisposable[] subscriptions)
            {
                this.isDisposed = false;
                this.subscriptions = new HashSet<IDisposable>();
                    
                this.AddRange(subscriptions);
            }

            public int Count
            {
                get { return this.subscriptions.Count; }
            }

            public void Dispose()
            {
                if (this.isDisposed)
                {
                    return;
                }

                foreach (IDisposable subscription in this.subscriptions)
                {
                    subscription.Dispose();
                }

                this.isDisposed = true;
            }

            private void AddRange(IEnumerable<IDisposable> subscriptions)
            {
                if (subscriptions == null)
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    if (object.ReferenceEquals(this, subscription))
                    {
                        continue;
                    }

                    CompositeSubscription other = subscription as CompositeSubscription;

                    if (other != null)
                    {
                        this.AddRange(other.subscriptions);
                    }
                    else
                    {
                        this.subscriptions.Add(subscription);
                    }
                }
            }
        }
    }
}