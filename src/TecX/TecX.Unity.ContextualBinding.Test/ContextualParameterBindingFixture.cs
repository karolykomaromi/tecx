namespace TecX.Unity.ContextualBinding.Test
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;
    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class ContextualParameterBindingFixture
    {
        [TestMethod]
        public void CanBindParameterOverrideInScope()
        {
            var container = new UnityContainer();

            container.RegisterType<IMyService, WritesToDatabaseService>(
                new InjectionConstructor("1"),
                new DestinationToConnectionStringBinding("http://localhost/service", "2"));

            var instanceContext = new InstanceContext();
            var message = new Message { Headers = new MessageHeaders { To = new Uri("http://localhost/service") } };

            IMyService service;
            using (new ContextScope(container, new ContextInfo("instanceContext", instanceContext), new ContextInfo("message", message)))
            {
                service = container.Resolve<IMyService>();

                Assert.AreEqual("2", service.ConnectionString);
            }

            service = container.Resolve<IMyService>();
            Assert.AreEqual("1", service.ConnectionString);
        }

        [TestMethod]
        public void CanOverrideMultipleParameters()
        {
            var container = new UnityContainer();

            container.RegisterType<IMyService, TakesTwoConnectionStrings>(
                new InjectionConstructor("1", "2"),
                new DestinationToConnectionStringBinding("http://localhost/service", "3"),
                new DestinationToConnectionStringBinding("http://localhost/service", "connectionString2", "4"));

            var instanceContext = new InstanceContext();
            var message = new Message { Headers = new MessageHeaders { To = new Uri("http://localhost/service") } };

            IMyService service;
            using (new ContextScope(container, new ContextInfo("instanceContext", instanceContext), new ContextInfo("message", message)))
            {
                service = container.Resolve<IMyService>();

                Assert.AreEqual("3", service.ConnectionString);
                Assert.AreEqual("4", service.ConnectionString2);
            }

            service = container.Resolve<IMyService>();
            Assert.AreEqual("1", service.ConnectionString);
            Assert.AreEqual("2", service.ConnectionString2);
        }
    }

    public class DestinationToConnectionStringBinding : InjectionMember
    {
        private readonly string url;

        private readonly string paramName;

        private readonly string connectionString;

        public DestinationToConnectionStringBinding(string url, string connectionString)
            : this(url, "connectionString", connectionString)
        {
        }

        public DestinationToConnectionStringBinding(string url, string paramName, string connectionString)
        {
            Guard.AssertNotEmpty(url, "url");
            Guard.AssertNotEmpty(paramName, "paramName");
            Guard.AssertNotEmpty(connectionString, "connectionString");

            this.url = url;
            this.paramName = paramName;
            this.connectionString = connectionString;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");

            NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);

            IContextualParameterBindingPolicy policy = policies.Get<IContextualParameterBindingPolicy>(key);

            if (policy == null)
            {
                policy = new ContextualParameterBindingPolicy();
                policies.Set<IContextualParameterBindingPolicy>(policy, key);
            }

            policy.Add(new DestinationToConnectionParameterOverride(this.url, this.paramName, this.connectionString));
        }
    }

    public class DestinationToConnectionParameterOverride : ContextualResolverOverride
    {
        private readonly Uri url;

        private readonly string paramName;

        private readonly string connectionString;

        public DestinationToConnectionParameterOverride(string url, string paramName, string connectionString)
        {
            Guard.AssertNotEmpty(url, "url");
            Guard.AssertNotEmpty(paramName, "paramName");
            Guard.AssertNotEmpty(connectionString, "connectionString");

            this.url = new Uri(url);
            this.paramName = paramName;
            this.connectionString = connectionString;
        }

        public override bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");
            Guard.AssertNotNull(builderContext, "builderContext");

            Message message = bindingContext["message"] as Message;

            if (message == null ||
                message.Headers == null ||
                message.Headers.To == null)
            {
                return false;
            }

            return message.Headers.To == this.url;
        }

        public override void SetResolverOverrides(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "builderContext");

            context.AddResolverOverrides(new[] { new ParameterOverride(this.paramName, this.connectionString), });
        }
    }
}
