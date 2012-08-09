namespace TecX.Web.Unity
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using TecX.Common;

    public class ContextCapturingControllerActivator : IHttpControllerActivator
    {
        private readonly IHttpControllerActivator activator;

        private readonly Func<TaskCompletionSource<HttpControllerContext>> promiseFactory;

        public ContextCapturingControllerActivator(Func<TaskCompletionSource<HttpControllerContext>> promiseFactory, IHttpControllerActivator activator)
        {
            Guard.AssertNotNull(promiseFactory, "promiseFactory");
            Guard.AssertNotNull(activator, "activator");

            this.activator = activator;
            this.promiseFactory = promiseFactory;
        }

        public IHttpController Create(HttpControllerContext controllerContext, Type controllerType)
        {
            Guard.AssertNotNull(controllerContext, "controllerContext");
            Guard.AssertNotNull(controllerContext, "controllerContext");

            this.promiseFactory().SetResult(controllerContext);
            return this.activator.Create(controllerContext, controllerType);
        }
    }
}
