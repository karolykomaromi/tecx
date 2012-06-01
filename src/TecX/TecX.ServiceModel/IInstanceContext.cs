namespace TecX.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;

    public interface IInstanceContext
    {
        event EventHandler Closed;

        event EventHandler Closing;

        event EventHandler Faulted;

        event EventHandler Opened;

        event EventHandler Opening;

        CommunicationState State { get; }

        IExtensionCollection<InstanceContext> Extensions { get; }

        ICollection<IChannel> IncomingChannels { get; }

        ICollection<IChannel> OutgoingChannels { get; }

        ServiceHostBase Host { get; }

        int ManualFlowControlLimit { get; set; }

        SynchronizationContext SynchronizationContext { get; set; }

        void Abort();

        IAsyncResult BeginClose(AsyncCallback callback, object state);

        IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state);

        IAsyncResult BeginOpen(AsyncCallback callback, object state);

        IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state);

        void Close();

        void Close(TimeSpan timeout);

        void EndClose(IAsyncResult result);

        void EndOpen(IAsyncResult result);

        void Open();

        void Open(TimeSpan timeout);

        object GetServiceInstance();

        object GetServiceInstance(Message message);

        int IncrementManualFlowControlLimit(int incrementBy);

        void ReleaseServiceInstance();
    }
}