namespace TecX.Web.Unity
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Microsoft.Practices.Unity;

    using TecX.Unity.Decoration;
    using TecX.Unity.Factories;

    class CompositionRoot
    {
        public CompositionRoot()
        {
            var container = new UnityContainer();

            container.AddNewExtension<DecoratorExtension>();

            container.RegisterType<IHttpControllerActivator, ContextCapturingControllerActivator>(new ContainerControlledLifetimeManager());

            container.RegisterType<IHttpControllerActivator, DefaultHttpControllerActivator>(new ContainerControlledLifetimeManager());

            container.RegisterType<Func<TaskCompletionSource<HttpControllerContext>>>(new ContainerControlledLifetimeManager(), new DelegateFactory());

            container.RegisterType<TaskCompletionSource<HttpControllerContext>>(new PerWebRequestLifetimeManager());

            container.RegisterType<HttpControllerContext>(
                new PerWebRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<TaskCompletionSource<HttpControllerContext>>().Task.Result));
        }
    }
}
