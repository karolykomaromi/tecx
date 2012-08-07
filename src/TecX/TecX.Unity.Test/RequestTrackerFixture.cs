namespace TecX.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;
    using TecX.Unity.Test.TestObjects;

    using Xunit;

    public class RequestTrackerFixture
    {
        [Fact]
        public void CanIdentifyParameterResolutionByBuildOperation()
        {
            var container = new UnityContainer();
            container.AddNewExtension<RequestTracker>();

            container.RegisterType<IFoo, Foo>();
            container.RegisterType<IBar, Bar2>();

            Consumer2 consumer = container.Resolve<Consumer2>();

            consumer = container.Resolve<Consumer2>();
        }
    }

    public class Consumer2
    {
        public IFoo Foo { get; set; }

        public IBar Bar { get; set; }

        public Consumer2(IFoo foo, IBar bar)
        {
            this.Foo = foo;
            this.Bar = bar;
        }
    }

    public class RequestTracker : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new CreationStrategy(), UnityBuildStage.Setup);
            this.Context.Strategies.Add(new RequestTrackerStrategy(), UnityBuildStage.PreCreation);
        }
    }

    public class CreationStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
        }
    }

    public interface IRequest
    {
        Type Service { get; }

        IRequest ParentRequest { get; }

        IBuilderContext Context { get; }

        ITarget Target { get; }

        int Depth { get; }

        IRequest CreateChild(Type service, IBuilderContext context, ITarget target);
    }

    [DebuggerDisplay("Service:'{Service}' Depth:'{Depth}'")]
    public class Request : IRequest
    {
        private WeakReference context;

        public Request(Type service)
        {
            Guard.AssertNotNull(service, "service");

            this.Service = service;
            this.Depth = 0;
        }

        public Request(IRequest parentRequest, IBuilderContext context, Type service, ITarget target)
        {
            Guard.AssertNotNull(parentRequest, "parentRequest");
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(service, "service");
            Guard.AssertNotNull(target, "target");

            this.Context = context;
            this.ParentRequest = parentRequest;
            this.Service = service;
            this.Target = target;
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

        public IRequest CreateChild(Type service, IBuilderContext context, ITarget target)
        {
            return new Request(this, context, service, target);
        }
    }

    public class RequestTrackerStrategy : BuilderStrategy
    {
        [ThreadStatic]
        private static IRequest request;

        private readonly Dictionary<NamedTypeBuildKey, ParameterInfo> lookup;

        public RequestTrackerStrategy()
        {
            this.lookup = new Dictionary<NamedTypeBuildKey, ParameterInfo>();
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            ParameterInfo parameter;
            if (this.lookup.TryGetValue(context.BuildKey, out parameter))
            {
                return;
            }

            IPolicyList destination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out destination);

            if (selector as SelectedConstructorCache != null)
            {
                return;
            }

            if (selector != null)
            {
                SelectedConstructor selectedConstructor = selector.SelectConstructor(context, destination);

                ConstructorInfo constructor = selectedConstructor.Constructor;

                ParameterInfo[] parameters = constructor.GetParameters();
                string[] keys = selectedConstructor.GetParameterKeys();

                //for (int i = 0; i < parameters.Length; i++)
                //{
                //    this.lookup.Add(new NamedTypeBuildKey(parameters[i].ParameterType, keys[i]), parameters[i]);
                //}

                for (int i = 0; i < parameters.Length; i++)
                {
                    var resolver = context.Policies.Get<IDependencyResolverPolicy>(keys[i]) as NamedTypeDependencyResolverPolicy;

                    if (resolver != null)
                    {
                        this.lookup.Add(new NamedTypeBuildKey(resolver.Type, resolver.Name), parameters[i]);
                    }

                    //context.Policies.Set<IDependencyResolverPolicy>(new NotifyingDependencyResolver(resolver, constructor, parameters[i], this.Callback), keys[i]);
                }

                destination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
            }
        }

        //private void Callback(MethodBase method, ParameterInfo parameter, IBuilderContext context)
        //{
        //    if (request == null)
        //    {
        //        request = new Request(method.DeclaringType);

        //        request = request.CreateChild(method.DeclaringType, context, new ParameterTarget(method, parameter));
        //    }
        //    else
        //    {
        //        IRequest current = request;

        //        while (current != null &&
        //            current.Service == method.DeclaringType &&
        //            current.Target != null)
        //        {
        //            current = current.ParentRequest;
        //        }

        //        if (current != null)
        //        {
        //            request = current.CreateChild(method.DeclaringType, context, new ParameterTarget(method, parameter));
        //        }
        //        else
        //        {
        //            request = new Request(method.DeclaringType);
        //            request = request.CreateChild(method.DeclaringType, context, new ParameterTarget(method, parameter));
        //        }
        //    }
        //}
    }

    public interface ITarget
    {
        Type Type { get; }

        string Name { get; }

        MemberInfo Member { get; }
    }

    [DebuggerDisplay("Name:'{Name}' Type:'{Type}'")]
    public abstract class Target<T> : ITarget
        where T : ICustomAttributeProvider
    {
        protected Target(MemberInfo member, T site)
        {
            Guard.AssertNotNull(member, "member");
            Guard.AssertNotNull(site, "site");

            this.Member = member;
            this.Site = site;
        }

        public MemberInfo Member { get; private set; }

        public T Site { get; private set; }

        public abstract string Name { get; }

        public abstract Type Type { get; }
    }

    public class PropertyTarget : Target<PropertyInfo>
    {
        public PropertyTarget(PropertyInfo site)
            : base(site, site)
        {
        }

        public override string Name
        {
            get { return Site.Name; }
        }

        public override Type Type
        {
            get { return Site.PropertyType; }
        }
    }

    public class ParameterTarget : Target<ParameterInfo>
    {
        public ParameterTarget(MethodBase method, ParameterInfo site)
            : base(method, site)
        {
        }

        public override string Name
        {
            get { return Site.Name; }
        }

        public override Type Type
        {
            get { return Site.ParameterType; }
        }
    }

    //public class NotifyingDependencyResolver : IDependencyResolverPolicy
    //{
    //    private readonly IDependencyResolverPolicy inner;

    //    private readonly MethodBase method;

    //    private readonly ParameterInfo parameter;

    //    private readonly Action<MethodBase, ParameterInfo, IBuilderContext> callback;

    //    public NotifyingDependencyResolver(IDependencyResolverPolicy inner, MethodBase method, ParameterInfo parameter, Action<MethodBase, ParameterInfo, IBuilderContext> callback)
    //    {
    //        Guard.AssertNotNull(inner, "inner");
    //        Guard.AssertNotNull(callback, "callback");

    //        this.inner = inner;
    //        this.method = method;
    //        this.parameter = parameter;
    //        this.callback = callback;
    //    }

    //    public object Resolve(IBuilderContext context)
    //    {
    //        this.callback(this.method, this.parameter, context);

    //        return this.inner.Resolve(context);
    //    }
    //}
}
