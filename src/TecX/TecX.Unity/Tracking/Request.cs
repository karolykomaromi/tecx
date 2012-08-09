namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    [DebuggerDisplay("Service:'{Service}' Depth:'{Depth}'")]
    public class Request : IRequest
    {
        private readonly ITarget[] availableTargets;

        private WeakReference context;

        private int currentTarget;

        public Request(Type service, IBuilderContext context, IEnumerable<ITarget> availableTargets)
        {
            Guard.AssertNotNull(service, "service");
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(availableTargets, "availableTargets");

            this.Service = service;
            this.Context = context;
            this.availableTargets = availableTargets.ToArray();
            this.Depth = 0;
            this.currentTarget = 0;
        }

        public Request(IRequest parentRequest, Type service, IBuilderContext context, IEnumerable<ITarget> availableTargets)
            : this(service, context, availableTargets)
        {
            Guard.AssertNotNull(parentRequest, "parentRequest");

            this.ParentRequest = parentRequest;

            this.Target = this.ParentRequest.GetNextTarget();

            this.Depth = this.ParentRequest.Depth + 1;
        }

        public Type Service { get; private set; }

        public IRequest ParentRequest { get; private set; }

        public IBuilderContext Context
        {
            get
            {
                if (this.context.IsAlive)
                {
                    return (IBuilderContext)this.context.Target;
                }

                return null;
            }

            private set
            {
                this.context = new WeakReference(value);
            }
        }

        public ITarget Target { get; private set; }

        public int Depth { get; private set; }

        public ITarget GetNextTarget()
        {
            if (this.currentTarget < this.availableTargets.Length - 1)
            {
                return this.availableTargets[this.currentTarget++];
            }

            return null;
        }

        public IRequest CreateChild(Type service, IBuilderContext context, IEnumerable<ITarget> targets)
        {
            return new Request(this, service, context, targets);
        }
    }
}