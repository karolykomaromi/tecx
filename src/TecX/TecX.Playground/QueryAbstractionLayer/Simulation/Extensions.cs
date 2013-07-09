namespace TecX.Playground.QueryAbstractionLayer.Simulation
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Playground.QueryAbstractionLayer.PD;

    public static class Extensions
    {
        public static IQueryable<T> Query<T>(this ISession session, PDIteratorOperator pdOperator = null, IClientInfo clientInfo = null)
            where T : PersistentObject
        {
            pdOperator = pdOperator ?? new PDIteratorOperator();
            clientInfo = clientInfo ?? ClientInfo.GetClientInfo();

            var builder = new FooBuilder();

            IEnumerable<Foo> foos = builder.Build(10);

            IQueryable<Foo> rawQuery = foos.AsQueryable();

            IQueryable<Foo> interceptedQuery = new QueryInterceptor<Foo>(rawQuery, pdOperator, clientInfo);

            return interceptedQuery as IQueryable<T>;
        }
    }
}