namespace Hydra.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class Tests
    {
        [Fact]
        public void Should_Do_What_I_Want_Not_What_I_Say()
        {
            var container = new UnityContainer();

            container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(Tests).Assembly).ImplementingOpenGenericInterface(typeof(IQueryHandler<,>)),
                WithMappings.FromAllInterfaces);

            var actual = container.Resolve<IQueryHandler<MyQuery, MyResponse>>();

            Assert.NotNull(actual);
            Assert.IsType<MyHandler>(actual);
        }

        [Fact]
        public void Should_Call_Dispose()
        {
            using (var container = new UnityContainer().RegisterType<IDocumentStore>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => new EmbeddableDocumentStore())))
            {
                IDocumentStore store = container.Resolve<IDocumentStore>();
            }
        }
    }

    public interface IDocumentStore
    {
    }

    public class EmbeddableDocumentStore: IDocumentStore, IDisposable
    {
        public void Dispose()
        {
        }
    }

    public static class AllClassesExtensions
    {
        public static IEnumerable<Type> ImplementingOpenGenericInterface(this IEnumerable<Type> candidates, Type openGenericInterface)
        {
            Contract.Requires(candidates != null);
            Contract.Requires(openGenericInterface != null);
            Contract.Requires(openGenericInterface.IsInterface);
            Contract.Requires(openGenericInterface.IsGenericType);
            Contract.Requires(openGenericInterface.IsGenericTypeDefinition);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            return from candidate in candidates
                   from @interface in candidate.GetInterfaces()
                   where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == openGenericInterface
                   select candidate;
        }
    }

    public interface IQuery<out TResponse>
    {
    }

    public class MyQuery : IQuery<MyResponse>
    {
    }

    public class MyHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public MyResponse Handle(MyQuery query)
        {
            return new MyResponse();
        }
    }

    public class MyResponse
    {
    }

    public interface IQueryHandler<in TQuery, out TResponse> where TQuery : IQuery<TResponse>
    {
        TResponse Handle(TQuery query);
    }
}
