namespace TecX.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public delegate IUnitOfWork UnitOfWorkFactory(bool readOnly);

    public interface IUnitOfWork
    {
    }

    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(bool readOnly)
        {
            this.ReadOnly = readOnly;
        }

        public bool ReadOnly { get; set; }
    }

    public class Consumer
    {
        public Consumer(UnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }

        public UnitOfWorkFactory Factory { get; set; }
    }

    [TestClass]
    public class DelegateFactoryFixture
    {
        [TestMethod]
        public void CanCreateDelegate()
        {
            var container = new UnityContainer();

            container.RegisterType<UnitOfWorkFactory>(new DelegateFactory());

            var consumer = container.Resolve<Consumer>();

            var uow = consumer.Factory(true) as UnitOfWork;

            Assert.IsNotNull(uow);
            Assert.IsTrue(uow.ReadOnly);
        }

        [TestMethod]
        public void Test()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            Type delegateType = typeof(UnitOfWorkFactory);

            Delegate @delegate = GetDelegate(container, delegateType);

            UnitOfWork uow = @delegate.DynamicInvoke(true) as UnitOfWork;

            Assert.IsNotNull(uow);
            Assert.IsTrue(uow.ReadOnly);
        }

        public static Delegate GetDelegate(IUnityContainer container, Type delegateType)
        {
            MethodInfo method = delegateType.GetMethod("Invoke");

            // alle parametertypen des delegate aufsammeln
            ParameterInfo[] parameters = method.GetParameters();

            Type[] funcParameterTypes = parameters.Select(pi => pi.ParameterType).Union(new[] { method.ReturnType }).ToArray();

            Type funcType = Expression.GetFuncType(funcParameterTypes);

            // prepare expression for every parameter of the delegate
            ParameterExpression[] parameterExpressions =
                parameters.Select(pi => Expression.Parameter(pi.ParameterType, pi.Name)).ToArray();

            // get constructor ParameterOverride(string name, object value)
            ConstructorInfo ctor = typeof(ParameterOverride).GetConstructor(new[] { typeof(string), typeof(object) });

            List<Expression> @overrides = new List<Expression>();

            // prepare a ParameterOverride for every parameter of the delegate
            foreach (ParameterExpression parameterExpression in parameterExpressions)
            {
                ConstantExpression parameterName = Expression.Constant(parameterExpression.Name, typeof(string));

                NewExpression @new = Expression.New(ctor, new Expression[] { parameterName, Expression.Convert(parameterExpression, typeof(object)) });

                @overrides.Add(Expression.Convert(@new, typeof(ResolverOverride)));
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
                    Expression.NewArrayInit(typeof(ResolverOverride), @overrides)
                };

            MethodCallExpression containerResolve = Expression.Call(ctr, resolve, resolveParameters);

            // convert the result of the call to container.Resolve from object to the return type of the delegate
            UnaryExpression returnValue = Expression.Convert(containerResolve, method.ReturnType);
            
            // put all the pieces into a lambda expression
            LambdaExpression func = Expression.Lambda(funcType, returnValue, parameterExpressions);

            // and finally compile it
            Delegate @delegate = func.Compile();

            return @delegate;
        }

        [TestMethod]
        public void Blueprint()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            Func<bool, IUnitOfWork> func = readOnly =>
            {
                ParameterOverride po1 = new ParameterOverride("readOnly", readOnly);

                return container.Resolve<IUnitOfWork>(po1);
            };

            UnitOfWorkFactory factory = new UnitOfWorkFactory(func);

            UnitOfWork uow = factory(true) as UnitOfWork;

            Assert.IsNotNull(uow);
            Assert.IsTrue(uow.ReadOnly);
        }
    }

    public class DelegateFactory : InjectionMember
    {
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            throw new NotImplementedException();
        }
    }

    public class DelegateFactoryBuildPlanPolicy : IBuildPlanPolicy
    {
        public void BuildUp(IBuilderContext context)
        {
            if (context.Existing != null)
            {

            }
        }
    }
}
