namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Count={Count}")]
    public class CompositeObserver<T> : IObserver<T>
    {
        private readonly HashSet<IObserver<T>> observers;

        public CompositeObserver(params IObserver<T>[] observers)
        {
            this.observers = new HashSet<IObserver<T>>();

            this.AddRange(observers);
        }

        public int Count
        {
            get { return this.observers.Count; }
        }

        public void OnNext(T value)
        {
            foreach (IObserver<T> observer in this.observers)
            {
                observer.OnNext(value);
            }
        }

        public void OnError(Exception error)
        {
            foreach (IObserver<T> observer in this.observers)
            {
                observer.OnError(error);
            }
        }

        public void OnCompleted()
        {
            foreach (IObserver<T> observer in this.observers)
            {
                observer.OnCompleted();
            }
        }

        private void AddRange(IEnumerable<IObserver<T>> observers)
        {
            if (observers == null)
            {
                return;
            }

            foreach (var observer in observers)
            {
                if (object.ReferenceEquals(this, observer))
                {
                    continue;
                }

                CompositeObserver<T> other = observer as CompositeObserver<T>;

                if (other != null)
                {
                    this.AddRange(other.observers);
                }
                else
                {
                    this.observers.Add(observer);
                }
            }
        }
    }
}