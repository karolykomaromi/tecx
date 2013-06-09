namespace TecX.Expressions.Test.TestObjects
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ServiceModel;

    [SerializeExpressionsBehavior, ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MyService : IMyService
    {
        public Customer[] Query(Expression<Func<Customer, bool>> query)
        {
            var selector = query.Compile();

            return new[]
                       {
                           new Customer{Id= 1},
                           new Customer{Id= 2},
                           new Customer{Id= 3},
                           new Customer{Id= 4},
                           new Customer{Id= 5},
                           new Customer{Id= 6}
                       }.Where(selector).ToArray();
        }
    }
}