namespace TecX.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.ServiceModel.Security;

    using TecX.Common;

    public class OperationContextWrapper : IOperationContext
    {
        private readonly OperationContext context;

        public OperationContextWrapper(OperationContext context)
        {
            Guard.AssertNotNull(context, "context");

            this.context = context;
        }

        public event EventHandler OperationCompleted
        {
            add
            {
                this.context.OperationCompleted += value;
            }

            remove
            {
                this.context.OperationCompleted -= value;
            }
        }

        public IContextChannel Channel
        {
            get
            {
                return this.context.Channel;
            }
        }

        public EndpointDispatcher EndpointDispatcher
        {
            get
            {
                return this.context.EndpointDispatcher;
            }

            set
            {
                this.context.EndpointDispatcher = value;
            }
        }

        public bool IsUserContext
        {
            get
            {
                return this.context.IsUserContext;
            }
        }

        public IExtensionCollection<OperationContext> Extensions
        {
            get
            {
                return this.context.Extensions;
            }
        }

        public bool HasSupportingTokens
        {
            get
            {
                return this.context.HasSupportingTokens;
            }
        }

        public ServiceHostBase Host
        {
            get
            {
                return this.context.Host;
            }
        }

        public MessageHeaders OutgoingMessageHeaders
        {
            get
            {
                return this.context.OutgoingMessageHeaders;
            }
        }

        public MessageProperties OutgoingMessageProperties
        {
            get
            {
                return this.context.OutgoingMessageProperties;
            }
        }

        public MessageHeaders IncomingMessageHeaders
        {
            get
            {
                return this.context.IncomingMessageHeaders;
            }
        }

        public MessageProperties IncomingMessageProperties
        {
            get
            {
                return this.context.IncomingMessageProperties;
            }
        }

        public MessageVersion IncomingMessageVersion
        {
            get
            {
                return this.context.IncomingMessageVersion;
            }
        }

        public IInstanceContext InstanceContext
        {
            get
            {
                return new InstanceContextWrapper(this.context.InstanceContext);
            }
        }

        public RequestContext RequestContext
        {
            get
            {
                return this.context.RequestContext;
            }

            set
            {
                this.context.RequestContext = value;
            }
        }

        public ServiceSecurityContext ServiceSecurityContext
        {
            get
            {
                return this.context.ServiceSecurityContext;
            }
        }

        public string SessionId
        {
            get
            {
                return this.context.SessionId;
            }
        }

        public ICollection<SupportingTokenSpecification> SupportingTokens
        {
            get
            {
                return this.context.SupportingTokens;
            }
        }

        public T GetCallbackChannel<T>()
        {
            return this.context.GetCallbackChannel<T>();
        }

        public void SetTransactionComplete()
        {
            this.context.SetTransactionComplete();
        }
    }
}