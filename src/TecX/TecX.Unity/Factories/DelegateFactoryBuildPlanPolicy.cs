namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class DelegateFactoryBuildPlanPolicy : IBuildPlanPolicy
    {
        private static class MethodNames
        {
            public const string Invoke = "Invoke";
        }

        private readonly Type delegateType;

        public DelegateFactoryBuildPlanPolicy(Type delegateType)
        {
            Guard.AssertNotNull(delegateType, "delegateType");

            this.delegateType = delegateType;
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

        public static Delegate GetDelegate(IUnityContainer container, Type delegateType)
        {
            MethodInfo method = delegateType.GetMethod(MethodNames.Invoke);

            // alle parametertypen des delegate aufsammeln
            ParameterInfo[] parameters = method.GetParameters();

            Type[] funcParameterTypes = parameters.Select(pi => pi.ParameterType).Union(new[] { method.ReturnType }).ToArray();

            Type funcType = Expression.GetFuncType(funcParameterTypes);

            // prepare expression for every parameter of the delegate
            ParameterExpression[] parameterExpressions =
                parameters.Select(pi => Expression.Parameter(pi.ParameterType, pi.Name)).ToArray();

            // get constructor ParameterOverride(string name, object value)
            ConstructorInfo parameterOverrideCtor = typeof(ParameterOverride).GetConstructor(new[] { typeof(string), typeof(object) });

            if (parameterOverrideCtor == null)
            {
                throw new InvalidOperationException("Constructor ParameterOverride(string,object) not found on Type Microsoft.Practices.Unity.ParameterOverride");
            }

            List<Expression> resolverOverrides = new List<Expression>();

            // prepare a ParameterOverride for every parameter of the delegate
            foreach (ParameterExpression parameterExpression in parameterExpressions)
            {
                ConstantExpression parameterName = Expression.Constant(parameterExpression.Name, typeof(string));

                NewExpression @new = Expression.New(parameterOverrideCtor, new Expression[] { parameterName, Expression.Convert(parameterExpression, typeof(object)) });

                resolverOverrides.Add(Expression.Convert(@new, typeof(ResolverOverride)));
            }

            // find the method IUnityContainer.Resolve(Type type, string name, ResolverOverride[] resolverOverrides)
            MethodInfo resolve = typeof(IUnityContainer).GetMethod(
                "Resolve", new[] { typeof(Type), typeof(string), typeof(ResolverOverride[]) });

            ConstantExpression ctr = Expression.Constant(container);

            // prepare the parameters for the resolve method
            Expression[] resolveParameters = new Expression[]
                {
                    Expression.Constant(method.ReturnType, typeof(Type)), 
                    Expression.Constant((string)null, typeof(string)),
                    Expression.NewArrayInit(typeof(ResolverOverride), resolverOverrides)
                };

            MethodCallExpression callContainerResolve = Expression.Call(ctr, resolve, resolveParameters);

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
    }
}