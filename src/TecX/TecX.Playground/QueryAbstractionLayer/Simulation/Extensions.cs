namespace TecX.Playground.QueryAbstractionLayer.Simulation
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Utility;
    using TecX.Playground.QueryAbstractionLayer.Visitors;

    public static class Extensions
    {
        public static IQueryable<T> Query<T>(
            this ISession session, 
            PDIteratorOperator pdOperator = null,
            IClientInfo clientInfo = null, 
            VisitorCache frameworkFilters = null,
            VisitorCache whereClauses = null)
            where T : PersistentObject
        {
            var builder = new FooBuilder();

            IEnumerable<Foo> foos = builder.Build(10);

            IQueryable<Foo> rawQuery = foos.AsQueryable();

            IQueryable<Foo> interceptedQuery = rawQuery.Intercept(pdOperator, clientInfo, frameworkFilters, whereClauses);

            return interceptedQuery as IQueryable<T>;
        }

        public static IQueryable<T> Intercept<T>(
            this IQueryable<T> rawQuery, 
            PDIteratorOperator pdOperator = null,
            IClientInfo clientInfo = null, 
            VisitorCache frameworkFilters = null,
            VisitorCache whereClauses = null)
            where T : PersistentObject
        {
            pdOperator = pdOperator ?? new PDIteratorOperator();
            clientInfo = clientInfo ?? ClientInfo.GetClientInfo();
            frameworkFilters = frameworkFilters ?? new VisitorCache(ExpressionHelper.CreateAppendFrameworkFiltersVisitor);
            whereClauses = whereClauses ?? new VisitorCache(ExpressionHelper.CreatePrependWhereClauseVisitor);

            IQueryable<T> interceptedQuery = new QueryInterceptor<T>(rawQuery, pdOperator, clientInfo, frameworkFilters, whereClauses);

            return interceptedQuery;
        }
    }
}