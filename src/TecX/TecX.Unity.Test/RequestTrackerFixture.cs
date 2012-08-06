namespace TecX.Unity.Test
{
    using System;
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
        }
    }

    public class Consumer2
    {
        public IFoo Foo { get; set; }

        public IBar Bar { get; set; }

        public Consumer2(IFoo foo, IBar bar)
        {
            Foo = foo;
            Bar = bar;
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

    public class RequestTrackerStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            IPolicyList destination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out destination);

            if (selector != null)
            {
                SelectedConstructor selectedConstructor = selector.SelectConstructor(context, destination);

                ConstructorInfo constructor = selectedConstructor.Constructor;

                ParameterInfo[] parameters = selectedConstructor.Constructor.GetParameters();
                string[] keys = selectedConstructor.GetParameterKeys();

                for (int i = 0; i < parameters.Length; i++)
                {
                    IDependencyResolverPolicy resolver = context.Policies.Get<IDependencyResolverPolicy>(keys[i]);

                    context.Policies.Set<IDependencyResolverPolicy>(new NotifyingDependencyResolver(resolver, constructor, parameters[i], this.Callback), keys[i]);
                }

                destination.Set<IConstructorSelectorPolicy>(
                    new SelectedConstructorCache(selectedConstructor), context.BuildKey);
            }
        }

        private void Callback(MethodBase method, ParameterInfo parameter)
        {

        }
    }

    public class NotifyingDependencyResolver : IDependencyResolverPolicy
    {
        private readonly IDependencyResolverPolicy inner;

        private readonly MethodBase method;

        private readonly ParameterInfo parameter;

        private readonly Action<MethodBase, ParameterInfo> callback;

        public NotifyingDependencyResolver(IDependencyResolverPolicy inner, MethodBase method, ParameterInfo parameter, Action<MethodBase, ParameterInfo> callback)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(callback, "callback");

            this.inner = inner;
            this.method = method;
            this.parameter = parameter;
            this.callback = callback;
        }

        public object Resolve(IBuilderContext context)
        {
            this.callback(this.method, this.parameter);

            return this.inner.Resolve(context);
        }
    }
}
