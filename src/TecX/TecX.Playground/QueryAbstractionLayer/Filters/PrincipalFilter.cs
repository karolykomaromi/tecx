namespace TecX.Playground.QueryAbstractionLayer.Filters
{
    using System;
    using System.Linq.Expressions;

    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Utility;

    public abstract class PrincipalFilter
    {
        public static readonly PrincipalFilter Enabled = new EnablePrincipalFilter();

        public static readonly PrincipalFilter Disabled = new DisablePrincipalFilter();

        public abstract Expression<Func<TElement, bool>> Filter<TElement>(IClientInfo clientInfo)
            where TElement : PersistentObject;

        private class DisablePrincipalFilter : PrincipalFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>(IClientInfo clientInfo)
            {
                return ExpressionHelper.AlwaysTrue<TElement>();
            }
        }

        private class EnablePrincipalFilter : PrincipalFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>(IClientInfo clientInfo)
            {
                ParameterExpression p = Expression.Parameter(typeof(TElement), "p");

                // call to nested property
                MemberExpression principalId = Expression.Property(Expression.Property(p, "Principal"), "PDO_ID");
                
                ConstantExpression pid = Expression.Constant(clientInfo.Principal != null ? clientInfo.Principal.PDO_ID : 0, typeof(long));

                BinaryExpression equals = Expression.Equal(principalId, pid);

                Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(equals, p);

                return filter;
            }
        }
    }
}