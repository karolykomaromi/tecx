namespace TecX.Unity.Proxies.Test.TestObjects
{
    using System;
    using System.ServiceModel;

    public class FooService : IFooService, ICommunicationObject
    {
        public FooService()
        {
            this.State = CommunicationState.Opened;
        }

        public event EventHandler Closed = delegate { };

        public event EventHandler Closing = delegate { };

        public event EventHandler Faulted = delegate { };

        public event EventHandler Opened = delegate { };

        public event EventHandler Opening = delegate { };

        public CommunicationState State { get; private set; }

        public void Abort()
        {
        }

        public void Close()
        {
        }

        public void Close(TimeSpan timeout)
        {
        }

        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            return null;
        }

        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        public void EndClose(IAsyncResult result)
        {
        }

        public void Open()
        {
        }

        public void Open(TimeSpan timeout)
        {
        }

        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            return null;
        }

        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        public void EndOpen(IAsyncResult result)
        {
        }

        public string Foo()
        {
            return "Foo()";
        }
    }
}