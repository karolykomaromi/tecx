namespace TecX.Unity.ContextualBinding.Test
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;

    [TestClass]
    public class ContextualParameterBindingFixture
    {
        [TestMethod]
        public void CanBindParameterOverrideInScope()
        {
            var container = new UnityContainer();

            container.RegisterType<IMyService, WritesToDatabaseService>(new InjectionConstructor("1"), new DestinationToConnectionStringBinding("http://localhost/service", "2"));

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
    }

    public class InstanceContext
    {

    }

    public class Message
    {
        public MessageHeaders Headers { get; set; }
    }

    public class MessageHeaders
    {
        public Uri To { get; set; }
    }

    public class DestinationToConnectionStringBinding : InjectionMember
    {
        private readonly string url;

        private readonly string connectionString;

        public DestinationToConnectionStringBinding(string url, string connectionString)
        {
            this.url = url;
            this.connectionString = connectionString;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            policies.Set<IContextualParameterBindingPolicy>(new DestinationToConnectionStringPolicy(this.url, this.connectionString), new NamedTypeBuildKey(implementationType, name));
        }
    }

    public class DestinationToConnectionStringPolicy : IContextualParameterBindingPolicy
    {
        private readonly Uri url;

        private readonly string connectionString;

        public DestinationToConnectionStringPolicy(string url, string connectionString)
        {
            Guard.AssertNotEmpty(url, "url");
            Guard.AssertNotEmpty(connectionString, "connectionString");

            this.url = new Uri(url);
            this.connectionString = connectionString;
        }

        public bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext)
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

        public void SetResolverOverrides(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "builderContext");

            context.AddResolverOverrides(new[] { new ParameterOverride("connectionString", this.connectionString), });
        }
    }

    public class WritesToDatabaseService : IMyService
    {
        public string ConnectionString { get; set; }

        public WritesToDatabaseService(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }

    public interface IMyService
    {
        string ConnectionString { get; set; }
    }
}
