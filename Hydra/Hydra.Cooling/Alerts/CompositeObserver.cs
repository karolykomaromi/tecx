namespace Hydra.Cooling.Alerts
{
    using System;

    public class CompositeObserver<T> : IObserver<T>
    {
        private readonly IObserver<T>[] observers;

        public CompositeObserver(params IObserver<T>[] observers)
        {
            this.observers = observers ?? new IObserver<T>[0];
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
    }
}