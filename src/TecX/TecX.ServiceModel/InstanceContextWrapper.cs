namespace TecX.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;

    using TecX.Common;

    public class InstanceContextWrapper : IInstanceContext
    {
        private readonly InstanceContext instanceContext;

        public InstanceContextWrappers(InstanceContext instanceContext)
        {
            Guard.AssertNotNull(instanceContext, "instanceContext");

            this.instanceContext = instanceContext;
        }

        public event EventHandler Closed
        {
            add
            {
                this.instanceContext.Closed += value;
            }
            remove
            {
                this.instanceContext.Closed -= value;
            }
        }

        public event EventHandler Closing
        {
            add
            {
                this.instanceContext.Closing += value;
            }
            remove
            {
                this.instanceContext.Closing -= value;
            }
        }

        public event EventHandler Faulted
        {
            add
            {
                this.instanceContext.Faulted += value;
            }
            remove
            {
                this.instanceContext.Faulted -= value;
            }
        }

        public event EventHandler Opened
        {
            add
            {
                this.instanceContext.Opened += value;
            }
            remove
            {
                this.instanceContext.Opened -= value;
            }
        }

        public event EventHandler Opening
        {
            add
            {
                this.instanceContext.Opening += value;
            }
            remove
            {
                this.instanceContext.Opening -= value;
            }
        }

        public IExtensionCollection<InstanceContext> Extensions
        {
            get
            {
                return this.instanceContext.Extensions;
            }
        }

        public ICollection<IChannel> IncomingChannels
        {
            get
            {
                return this.instanceContext.IncomingChannels;
            }
        }

        public ICollection<IChannel> OutgoingChannels
        {
            get
            {
                return this.instanceContext.OutgoingChannels;
            }
        }

        public ServiceHostBase Host
        {
            get
            {
                return this.instanceContext.Host;
            }
        }
        
        public int ManualFlowControlLimit
        {
            get
            {
                return this.instanceContext.ManualFlowControlLimit;
            }

            set
            {
                this.instanceContext.ManualFlowControlLimit = value;
            }
        }

        public CommunicationState State
        {
            get
            {
                return this.instanceContext.State;
            }
        }

        public SynchronizationContext SynchronizationContext
        {
            get
            {
                return this.instanceContext.SynchronizationContext;
            }
            set
            {
                this.instanceContext.SynchronizationContext = value;
            }
        }
        
        public void Abort()
        {
            this.instanceContext.Abort();
        }

        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            return this.instanceContext.BeginClose(callback, state);
        }

        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return this.instanceContext.BeginClose(timeout, callback, state);
        }

        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            return this.instanceContext.BeginOpen(callback, state);
        }

        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return this.instanceContext.BeginOpen(timeout, callback, state);
        }

        public void Close()
        {
            this.instanceContext.Close();
        }

        public void Close(TimeSpan timeout)
        {
            this.instanceContext.Close(timeout);
        }

        public void EndClose(IAsyncResult result)
        {
            this.instanceContext.EndClose(result);
        }

        public void EndOpen(IAsyncResult result)
        {
            this.instanceContext.EndOpen(result);
        }

        public void Open()
        {
            this.instanceContext.Open();
        }

        public void Open(TimeSpan timeout)
        {
            this.instanceContext.Open(timeout);
        }

        public object GetServiceInstance()
        {
            return this.instanceContext.GetServiceInstance();
        }

        public object GetServiceInstance(Message message)
        {
            return this.instanceContext.GetServiceInstance(message);
        }

        public int IncrementManualFlowControlLimit(int incrementBy)
        {
            return this.instanceContext.IncrementManualFlowControlLimit(incrementBy);
        }

        public void ReleaseServiceInstance()
        {
            this.instanceContext.ReleaseServiceInstance();
        }
    }
}