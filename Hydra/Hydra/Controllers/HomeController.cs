namespace Hydra.Controllers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Web.Mvc;
    using Hydra.Models;

    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Index()
        {
            // return this.Content("Hello World!");
            Registration model = new Registration { Email = "foo@bar.local" };

            return this.View(model);
        }
    }

    public interface IQuery<out TResponse>
    {
    }

    public interface IQueryHandler<in TQuery, out TResponse>
        where TQuery : IQuery<TResponse>
    {
        TResponse Handle(TQuery query);
    }

    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> request);
    }

    public class Mediator : IMediator
    {
        private readonly IDependencyResolver dependencyResolver;

        public Mediator(IDependencyResolver dependencyResolver)
        {
            Contract.Requires(dependencyResolver != null);

            this.dependencyResolver = dependencyResolver;
        }

        public virtual TResponse Request<TResponse>(IQuery<TResponse> request)
        {
            var plan = new MediatorPlan<TResponse>(typeof(IQueryHandler<,>), "Handle", request.GetType(), this.dependencyResolver);

            return plan.Invoke(request);
        }

        private class MediatorPlan<TResult>
        {
            private readonly MethodInfo handleMethod;

            private readonly Func<object> handlerInstanceBuilder;

            public MediatorPlan(Type handlerTypeTemplate, string handlerMethodName, Type messageType, IDependencyResolver dependencyResolver)
            {
                var handlerType = handlerTypeTemplate.MakeGenericType(messageType, typeof(TResult));

                this.handleMethod = this.GetHandlerMethod(handlerType, handlerMethodName, messageType);

                this.handlerInstanceBuilder = () => dependencyResolver.GetService(handlerType);
            }

            public TResult Invoke(object message)
            {
                return (TResult)this.handleMethod.Invoke(this.handlerInstanceBuilder(), new[] { message });
            }

            private MethodInfo GetHandlerMethod(Type handlerType, string handlerMethodName, Type messageType)
            {
                return handlerType.GetMethod(
                    handlerMethodName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    null,
                    CallingConventions.HasThis,
                    new[] { messageType },
                    null);
            }
        }
    }
}