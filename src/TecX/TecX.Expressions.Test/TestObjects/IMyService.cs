namespace TecX.Expressions.Test.TestObjects
{
    using System;
    using System.Linq.Expressions;
    using System.ServiceModel;

    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        Customer[] Query(Expression<Func<Customer, bool>> query);
    }
}