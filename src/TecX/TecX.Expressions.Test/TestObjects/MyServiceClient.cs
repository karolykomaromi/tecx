namespace TecX.Expressions.Test.TestObjects
{
    using System;
    using System.Linq.Expressions;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class MyServiceClient : ClientBase<IMyService>, IMyService
    {
        public MyServiceClient(Binding binding, EndpointAddress address)
            : base(binding, address)
        {
            this.Endpoint.Behaviors.Add(new ClientSideSerializeExpressionsBehavior());
        }

        public Customer[] QueryCustomers(Expression<Func<Customer, bool>> query)
        {
            return this.Channel.QueryCustomers(query);
        }
    }
}