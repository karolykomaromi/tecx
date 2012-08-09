namespace TecX.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.ServiceModel.Security;

    public interface IOperationContext
    {
        event EventHandler OperationCompleted;

        IContextChannel Channel { get; }

        EndpointDispatcher EndpointDispatcher { get; set; }

        bool IsUserContext { get; }

        IExtensionCollection<OperationContext> Extensions { get; }

        bool HasSupportingTokens { get; }

        ServiceHostBase Host { get; }

        MessageHeaders OutgoingMessageHeaders { get; }

        MessageProperties OutgoingMessageProperties { get; }

        MessageHeaders IncomingMessageHeaders { get; }

        MessageProperties IncomingMessageProperties { get; }

        MessageVersion IncomingMessageVersion { get; }

        IInstanceContext InstanceContext { get; }

        RequestContext RequestContext { get; set; }

        ServiceSecurityContext ServiceSecurityContext { get; }

        string SessionId { get; }

        ICollection<SupportingTokenSpecification> SupportingTokens { get; }

        T GetCallbackChannel<T>();

        void SetTransactionComplete();
    }
}