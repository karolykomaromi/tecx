namespace TecX.Query.Simulation
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Query.PD;
    using TecX.Query.Strategies;
    using TecX.Query.Utility;
    using TecX.Query.Visitors;

    public static class Extensions
    {
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

            IQueryable<T> interceptedQuery = new QueryInterceptor<T>(
                rawQuery, 
                new QueryProviderInterceptor(
                    rawQuery.Provider, 
                    new Linq2ObjectStrategy(pdOperator, clientInfo, frameworkFilters, whereClauses)));

            return interceptedQuery;
        }
    }
}