namespace TecX.Expressions.Test
{
    using System;
    using System.ServiceModel;
    using TecX.Expressions.Test.TestObjects;
    using Xunit;

    public class SerializeExpressionsFixture
    {
        /// <summary>
        /// http://localhost:12345/myservice
        /// </summary>
        private const string ServiceAddress = "http://localhost:12345/myservice";

        [Fact]
        public void Should_Work_With_ClientBase_Derived_Proxy()
        {
            using (ServiceHost host = new ServiceHost(typeof(MyService), new Uri(ServiceAddress)))
            {
                host.Open();

                using (var proxy = new MyServiceClient(new BasicHttpBinding(), new EndpointAddress(ServiceAddress)))
                {
                    Customer[] actual = proxy.QueryCustomers(c => c.Id > 3);

                    Customer[] expected = new[]
                    {
                        new Customer {Id = 4},
                        new Customer {Id = 5},
                        new Customer {Id = 6}
                    };

                    Assert.Equal(expected, actual, new CustomerComparer());
                }
            }
        }

        [Fact]
        public void Should_Work_With_Factory_Generated_Proxy()
        {
            using (ServiceHost host = new ServiceHost(typeof(MyService), new Uri(ServiceAddress)))
            {
                host.Open();

                var factory = new ChannelFactory<IMyService>(new BasicHttpBinding(), ServiceAddress);

                factory.Endpoint.Behaviors.Add(new ClientSideSerializeExpressionsBehavior());

                var proxy = factory.CreateChannel();

                Customer[] actual = proxy.QueryCustomers(c => c.Id > 3);

                Customer[] expected = new[]
                    {
                        new Customer {Id = 4},
                        new Customer {Id = 5},
                        new Customer {Id = 6}
                    };

                Assert.Equal(expected, actual, new CustomerComparer());
            }
        }

        [Fact]
        public void Should_Work_With_AddReference_Generated_Proxy()
        {
            using (ServiceHost host = new ServiceHost(typeof(MyService), new Uri(ServiceAddress)))
            {
                host.Open();

                var factory = new ChannelFactory<IMyService>(new BasicHttpBinding(), ServiceAddress);

                factory.Endpoint.Behaviors.Add(new ClientSideSerializeExpressionsBehavior());

                var proxy = factory.CreateChannel();

                Customer[] actual = proxy.QueryCustomers(c => c.Id > 3);

                Customer[] expected = new[]
                    {
                        new Customer {Id = 4},
                        new Customer {Id = 5},
                        new Customer {Id = 6}
                    };

                Assert.Equal(expected, actual, new CustomerComparer());
            }
        }
    }
}
