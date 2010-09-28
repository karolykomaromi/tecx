using System.Reflection;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Registration;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class InterceptionOptionsBuilderFixture
    {
        [TestMethod]
        public void CanSetupInterceptionForInterface()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<Interception>();

            container.RegisterType<IInterceptable>(
                new DefaultInterceptor(new InterfaceInterceptor()),
                new DefaultInterceptionBehavior<IncrementIntegerByOneBehavior>());

            container.RegisterType<IInterceptable, Interceptable>();
            container.RegisterType<IInterceptable, AnotherInterceptable>("test2");

            IInterceptable testable = container.Resolve<IInterceptable>();

            testable.SetValue(1);

            Assert.AreEqual(2, testable.Value);

            testable = container.Resolve<IInterceptable>("test2");

            testable.SetValue(2);

            Assert.AreEqual(3, testable.Value);
        }

        [TestMethod]
        public void WhenRegisteringInterceptions_BehaviorAddsOneToEveryInteger()
        {
            IUnityContainer container = new UnityContainer();

            var registry = container
                .ConfigureAutoRegistration()
                .EnableInterception()
                .ExcludeSystemAssemblies()
                .ExcludeUnitTestAssemblies()
                .Include(If.Is<IInterceptable>(), Then.Intercept()
                                                      .AllImplementors()
                                                      .ByImplementingContract()
                                                      .WrapAllWith<IncrementIntegerByOneBehavior>())
                .Include(If.Implements<IInterceptable>(), Then.Register().WithTypeName())
                .ApplyAutoRegistrations();

            IInterceptable interceptable = container.Resolve<IInterceptable>("Interceptable");

            interceptable.SetValue(1);

            Assert.AreEqual(2, interceptable.Value);

            interceptable = container.Resolve<IInterceptable>("AnotherInterceptable");

            interceptable.SetValue(2);

            Assert.AreEqual(3, interceptable.Value);
        }

        [TestMethod]
        public void WhenDefiningAllImplementors_DefaultInterceptorIsSet()
        {
            var option = Then.Intercept()
                .AllImplementors()
                .ApplyForType(typeof(IInterceptable))
                .Build();

            Assert.AreEqual(typeof(DefaultInterceptor), option.Interceptor.GetType());
        }

        [TestMethod]
        public void WhenDefiningWrapAllWith_DefaultInterceptionBehaviorIsSet()
        {
            var option = Then.Intercept()
                .WrapAllWith<IncrementIntegerByOneBehavior>()
                .ApplyForType(typeof(IInterceptable))
                .Build();

            Assert.AreEqual(typeof (DefaultInterceptionBehavior<IncrementIntegerByOneBehavior>),
                            option.Behaviors.First().GetType());
        }

        [TestMethod]
        public void WhenDefiningWrapWith_InterceptionBehaviorIsSet()
        {
            var option = Then.Intercept()
                .WrapWith<IncrementIntegerByOneBehavior>()
                .ApplyForType(typeof(IInterceptable))
                .Build();

            Assert.AreEqual(typeof(InterceptionBehavior<IncrementIntegerByOneBehavior>),
                            option.Behaviors.First().GetType());
        }

        [TestMethod]
        public void WhenDefiningByImplementingContract_InterfaceInterceptorIsSet()
        {
            var option = Then.Intercept()
                .ByImplementingContract()
                .ApplyForType(typeof(IInterceptable))
                .Build();

            FieldInfo info = typeof(Interceptor).GetField("interceptor",
                                               BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(info);

            IInterceptor innerInterceptor =info.GetValue(option.Interceptor) as IInterceptor;

            Assert.IsNotNull(innerInterceptor);
            Assert.AreEqual(typeof (InterfaceInterceptor), innerInterceptor.GetType());
        }

        [TestMethod]
        public void WhenDefiningBySubclassing_VirtualMethodInterceptorIsSet()
        {
            var option = Then.Intercept()
                .BySubclassing()
                .ApplyForType(typeof(IInterceptable))
                .Build();

            FieldInfo info = typeof (Interceptor).GetField("interceptor",
                                                           BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(info);

            IInterceptor innerInterceptor = info.GetValue(option.Interceptor) as IInterceptor;

            Assert.IsNotNull(innerInterceptor);
            Assert.AreEqual(typeof(VirtualMethodInterceptor), innerInterceptor.GetType());
        }

        [TestMethod]
        public void WhenDefiningByMarshalling_TransparentProxyInterceptorIsSet()
        {
            var option = Then.Intercept()
                .ByMarshalling()
                .ApplyForType(typeof(IInterceptable))
                .Build();

            FieldInfo info = typeof(Interceptor).GetField("interceptor",
                                               BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(info);

            IInterceptor innerInterceptor = info.GetValue(option.Interceptor) as IInterceptor;

            Assert.IsNotNull(innerInterceptor);
            Assert.AreEqual(typeof(TransparentProxyInterceptor), innerInterceptor.GetType());
        }
    }
}
