namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class DelegateFactoryBuildPlanPolicy : IBuildPlanPolicy
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here (Constants).")]
        private static class MethodNames
        {
            /// <summary>
            /// Invoke
            /// </summary>
            public const string Invoke = "Invoke";

            /// <summary>
            /// Resolve
            /// </summary>
            public const string Resolve = "Resolve";
        }

        // get constructor ParameterOverride(string name, object value)
        private static readonly ConstructorInfo ParameterOverrideCtor =
            typeof(ParameterOverride).GetConstructor(new[] { typeof(string), typeof(object) });

        // get ctor OneTimeTypeMatchParameterOverride(object value)
        private static readonly ConstructorInfo OneTimeTypeMatchParameterOverrideCtor =
            typeof(OneTimeTypeMatchParameterOverride).GetConstructor(new[] { typeof(object) });

        // find the method IUnityContainer.Resolve(Type type, string name, ResolverOverride[] resolverOverrides)
        private static readonly MethodInfo ContainerResolveMethod =
            typeof(IUnityContainer).GetMethod(MethodNames.Resolve, new[] { typeof(Type), typeof(string), typeof(ResolverOverride[]) });

        private readonly Type delegateType;
        
        public DelegateFactoryBuildPlanPolicy(Type delegateType)
        {
            Guard.AssertNotNull(delegateType, "delegateType");

            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                throw new ArgumentException("'delegateType' must derive from 'System.Delegate'.", "delegateType");
            }

            this.delegateType = delegateType;
        }

        public static Delegate GetDelegate(IUnityContainer container, Type delegateType)
        {
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                throw new ArgumentException("'delegateType' must derive from 'System.Delegate'.", "delegateType");
            }

            bool isFunc = delegateType.FullName.StartsWith("System.Func", StringComparison.Ordinal);

            MethodInfo method = delegateType.GetMethod(MethodNames.Invoke);

            // alle parametertypen des delegate aufsammeln
            ParameterInfo[] parameters = method.GetParameters();

            Type[] funcParameterTypes = 
                parameters.Select(pi => pi.ParameterType).Concat(new[] { method.ReturnType }).ToArray();

            Type funcType = Expression.GetFuncType(funcParameterTypes);

            // prepare expression for every parameter of the delegate
            ParameterExpression[] parameterExpressions =
                parameters.Select(pi => Expression.Parameter(pi.ParameterType, pi.Name)).ToArray();
            
            IEnumerable<Expression> resolverOverrides = isFunc ? GetResolverOverridesForFunc(parameterExpressions) : GetResolverOverrides(parameterExpressions);

            ConstantExpression ctr = Expression.Constant(container);

            // prepare the parameters for the resolve method
            Expression[] resolveParameters = new Expression[]
                {
                    Expression.Constant(method.ReturnType, typeof(Type)),
                    Expression.Constant((string)null, typeof(string)),
                    Expression.NewArrayInit(typeof(ResolverOverride), resolverOverrides)
                };

            MethodCallExpression callContainerResolve = Expression.Call(ctr, ContainerResolveMethod, resolveParameters);

            // convert the result of the call to container.Resolve from object to the return type of the delegate
            UnaryExpression returnValue = Expression.Convert(callContainerResolve, method.ReturnType);

            // put all the pieces into a lambda expression
            LambdaExpression lambda = Expression.Lambda(funcType, returnValue, parameterExpressions);

            // and finally compile it
            Delegate func = lambda.Compile();

            // need to create the strongly typed delegate using the compiled lambda as parameter
            Delegate factoryDelegate = Delegate.CreateDelegate(delegateType, func, MethodNames.Invoke);

            return factoryDelegate;
        }

        public void BuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (context.Existing == null)
            {
                IUnityContainer container = context.NewBuildUp<IUnityContainer>();

                Delegate factoryDelegate = GetDelegate(container, this.delegateType);

                context.Existing = factoryDelegate;
            }
        }

        private static IEnumerable<Expression> GetResolverOverridesForFunc(IEnumerable<ParameterExpression> parameterExpressions)
        {
            List<Expression> resolverOverrides = new List<Expression>();
            
            foreach (ParameterExpression parameterExpression in parameterExpressions)
            {
                NewExpression @new = Expression.New(
                    OneTimeTypeMatchParameterOverrideCtor,
                    new Expression[] { Expression.Convert(parameterExpression, typeof(object)) });

                resolverOverrides.Add(Expression.Convert(@new, typeof(ResolverOverride)));
            }

            resolverOverrides.Reverse();

            return resolverOverrides;
        }

        private static IEnumerable<Expression> GetResolverOverrides(IEnumerable<ParameterExpression> parameterExpressions)
        {
            List<Expression> resolverOverrides = new List<Expression>();

            // prepare a ParameterOverride for every parameter of the delegate
            foreach (ParameterExpression parameterExpression in parameterExpressions)
            {
                ConstantExpression parameterName = Expression.Constant(parameterExpression.Name, typeof(string));

                NewExpression @new = Expression.New(
                    ParameterOverrideCtor,
                    new Expression[] { parameterName, Expression.Convert(parameterExpression, typeof(object)) });

                resolverOverrides.Add(Expression.Convert(@new, typeof(ResolverOverride)));
            }

            return resolverOverrides;
        }
    }
}