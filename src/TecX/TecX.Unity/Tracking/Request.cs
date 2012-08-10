namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    [DebuggerDisplay("Service:'{Service}' Depth:'{Depth}'")]
    public class Request : IRequest
    {
        private static readonly IDictionary<string, object> staticRequestContext;

        [ThreadStatic]
        private static IRequest current;

        private readonly ITarget[] availableTargets;

        private readonly WeakReference builderContext;

        private readonly NamedTypeBuildKey buildKey;

        private readonly int depth;

        private readonly NamedTypeBuildKey originalBuildKey;

        private readonly IRequest parentRequest;

        private readonly IDictionary<string, object> requestContext;

        private readonly Type service;

        private readonly ITarget target;

        private int currentTarget;

        static Request()
        {
            staticRequestContext = new Dictionary<string, object>();
        }

        public Request(Type service, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(service, "service");
            Guard.AssertNotNull(builderContext, "context");

            this.service = service;
            this.builderContext = new WeakReference(builderContext);

            this.availableTargets = GetAvailableTargets(builderContext);
            this.buildKey = builderContext.BuildKey;
            this.currentTarget = 0;
            this.depth = 0;
            this.originalBuildKey = builderContext.OriginalBuildKey;
            this.requestContext = new HierarchicalDictionary<string, object>(StaticRequestContext, new Dictionary<string, object>());
        }

        public Request(IRequest parentRequest, Type service, IBuilderContext builderContext, ITarget target)
            : this(service, builderContext)
        {
            Guard.AssertNotNull(parentRequest, "parentRequest");

            this.parentRequest = parentRequest;

            // target can actually be null if you have out-of-band calls to the container. These can occur
            // if you use InjectionFactories or similar constructs.
            this.target = target;

            this.depth = this.ParentRequest.Depth + 1;
            this.requestContext = this.ParentRequest.RequestContext;
        }

        public static IRequest Current
        {
            get
            {
                return current;
            }

            set
            {
                current = value;
            }
        }

        public static IDictionary<string, object> StaticRequestContext
        {
            get
            {
                return staticRequestContext;
            }
        }

        public IBuilderContext BuilderContext
        {
            get
            {
                if (this.builderContext.IsAlive)
                {
                    return (IBuilderContext)this.builderContext.Target;
                }

                return null;
            }
        }

        public int Depth
        {
            get
            {
                return this.depth;
            }
        }

        public IRequest ParentRequest
        {
            get
            {
                return this.parentRequest;
            }
        }

        public IDictionary<string, object> RequestContext
        {
            get
            {
                return this.requestContext;
            }
        }

        public Type Service
        {
            get
            {
                return this.service;
            }
        }

        public ITarget Target
        {
            get
            {
                return this.target;
            }
        }

        public NamedTypeBuildKey BuildKey
        {
            get
            {
                return this.buildKey;
            }
        }

        public NamedTypeBuildKey OriginalBuildKey
        {
            get
            {
                return this.originalBuildKey;
            }
        }

        public IRequest CreateChild(Type service, IBuilderContext context)
        {
            ITarget targetForChild = this.GetNextTarget();

            return new Request(this, service, context, targetForChild);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(250);

            sb.AppendFormat("Service:'{0}'", this.Service);
            sb.AppendFormat(" Depth:'{0}'", this.Depth);

            return sb.ToString();
        }

        private static ITarget[] GetAvailableTargets(IBuilderContext context)
        {
            IPolicyList destination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out destination);

            IEnumerable<ITarget> targets = new ITarget[0];

            if (selector != null)
            {
                SelectedConstructor selectedConstructor = selector.SelectConstructor(context, destination);

                if (selectedConstructor != null)
                {
                    ConstructorInfo constructor = selectedConstructor.Constructor;

                    targets =
                        targets.Concat(constructor.GetParameters().Select(p => new ParameterTarget(constructor, p)));

                    destination.Set<IConstructorSelectorPolicy>(
                        new SelectedConstructorCache(selectedConstructor), context.BuildKey);
                }
            }

            IPropertySelectorPolicy propertySelector = context.Policies.Get<IPropertySelectorPolicy>(context.BuildKey, out destination);

            if (propertySelector != null)
            {
                var selectedProperties = propertySelector.SelectProperties(context, destination).ToArray();
                targets = targets.Concat(selectedProperties.Select(p => new PropertyTarget(p.Property)));

                destination.Set<IPropertySelectorPolicy>(
                    new SelectedPropertiesCache(selectedProperties), context.BuildKey);
            }

            IMethodSelectorPolicy methodSelector = context.Policies.Get<IMethodSelectorPolicy>(context.BuildKey, out destination);

            if (methodSelector != null)
            {
                var selectedMethods = methodSelector.SelectMethods(context, destination).ToArray();

                foreach (var selectedMethod in selectedMethods)
                {
                    MethodInfo method = selectedMethod.Method;

                    targets = targets.Concat(method.GetParameters().Select(p => new ParameterTarget(method, p)));
                }

                destination.Set<IMethodSelectorPolicy>(new SelectedMethodsCache(selectedMethods), context.BuildKey);
            }

            return targets.ToArray();
        }

        private ITarget GetNextTarget()
        {
            //int i = this.currentTarget;
            //int m = this.availableTargets.Length;

            //i = ((i % m) + m) % m;

            if (this.currentTarget < this.availableTargets.Length)
            {
                ITarget t = this.availableTargets[this.currentTarget++];

                this.currentTarget = this.currentTarget % this.availableTargets.Length;

                return t;
            }

            return null;
        }
    }
}