namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows.Threading;

    public abstract class Alerting
    {
        private readonly List<WeakReference> eventHandlers;
        private readonly Dispatcher dispatcher;
        private bool hasCanExecuteQueued;

        protected Alerting(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
            this.eventHandlers = new List<WeakReference>();
        }

        protected void RaiseEvent()
        {
            if (this.hasCanExecuteQueued)
            {
                return;
            }

            this.hasCanExecuteQueued = true;

            this.dispatcher.BeginInvoke(
                () =>
                    {
                        this.CallWeakReferenceHandlers();
                        this.hasCanExecuteQueued = false;
                    });
        }

        protected void RemoveWeakReferenceHandler(EventHandler handler)
        {
            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];
                EventHandler target = reference.Target as EventHandler;
                if (target == null || 
                    target == handler)
                {
                    this.eventHandlers.RemoveAt(i);
                }
            }
        }

        protected void AddWeakReferenceHandler(EventHandler handler)
        {
            this.eventHandlers.Add(new WeakReference(handler));
        }

        private void CallWeakReferenceHandlers()
        {
            EventHandler[] handlerArray = new EventHandler[this.eventHandlers.Count];
            int index = 0;
            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];
                EventHandler target = reference.Target as EventHandler;
                if (target == null)
                {
                    this.eventHandlers.RemoveAt(i);
                }
                else
                {
                    handlerArray[index] = target;
                    index++;
                }
            }

            for (int j = 0; j < index; j++)
            {
                EventHandler handler2 = handlerArray[j];
                handler2(this, EventArgs.Empty);
            }
        }
    }
}