namespace Hydra.Test.Queries
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Queries;
    using Xunit;

    public class MediatorTests
    {
        [Fact]
        public void Should_Create_Func_To_Call_Handle_Method()
        {
            Func<MyQueryHandler, MyQuery, MyResponse> func = (h, q) => h.Handle(q);

            ParameterExpression handler = Expression.Parameter(typeof(MyQueryHandler), "handler");
            ParameterExpression query = Expression.Parameter(typeof(MyQuery), "query");

            MethodInfo handleMethod =
                typeof(IQueryHandler<,>).MakeGenericType(typeof(MyQuery), typeof(MyResponse)).GetMethod("Handle", new[] { typeof(MyQuery) });

            MethodCallExpression handle = Expression.Call(handler, handleMethod, query);

            Func<MyQueryHandler, MyQuery, MyResponse> x = Expression.Lambda<Func<MyQueryHandler, MyQuery, MyResponse>>(handle, handler, query).Compile();

            Assert.NotNull(x(new MyQueryHandler(), new MyQuery()));
        }
    }

    public class MyQueryHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public MyResponse Handle(MyQuery query)
        {
            return new MyResponse();
        }
    }

    public class MyQuery : IQuery<MyResponse>
    {
    }

    public class MyResponse
    {
    }
}
