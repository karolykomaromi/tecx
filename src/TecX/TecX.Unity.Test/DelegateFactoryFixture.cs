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
            Type delegateType = typeof(UnitOfWorkFactory);

            Delegate @delegate = GetDelegate(delegateType);

            object x = @delegate.DynamicInvoke(true);

            //Console.WriteLine(method.ReturnType.Name + " (ret)");
            //foreach (ParameterInfo param in method.GetParameters())
            //{
            //    Console.WriteLine("{0} {1}", param.ParameterType.Name, param.Name);
            //}



            // return type aufsammeln

            // func definieren mit parametertypen und return type

            // body definieren

            // neue parameteroverrides definieren mit parametern des delegate
        }

        public static Delegate GetDelegate(Type delegateType)
        {
            MethodInfo method = delegateType.GetMethod("Invoke");

            // alle parametertypen des delegate aufsammeln
            ParameterInfo[] parameters = method.GetParameters();

            Type[] funcParameterTypes = parameters.Select(pi => pi.ParameterType).Union(new[] { method.ReturnType }).ToArray();

            Type funcType = Expression.GetFuncType(funcParameterTypes);

            ParameterExpression[] parameterExpressions =
                parameters.Select(pi => Expression.Parameter(pi.ParameterType, pi.Name)).ToArray();

            ConstructorInfo ctor = typeof(ParameterOverride).GetConstructor(new[] { typeof(string), typeof(object) });

            List<Expression> @overrides = new List<Expression>();

            for (int i = 0; i < parameterExpressions.Length; i++)
            {
                ParameterExpression parameterExpression = parameterExpressions[i];

                ConstantExpression parameterName = Expression.Constant(parameterExpression.Name, typeof(string));

                NewExpression @new = Expression.New(ctor, new Expression[] { parameterName, Expression.Convert(parameterExpression, typeof(object)) });

                @overrides.Add(@new);
            }

            MethodInfo resolve = typeof(IUnityContainer).GetMethod(
                "Resolve", new[] { typeof(Type), typeof(string), typeof(ResolverOverride[]) });

            MethodCallExpression call = Expression.Call(resolve, @overrides.ToArray());

            LambdaExpression func = Expression.Lambda(funcType, call, parameterExpressions);

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
