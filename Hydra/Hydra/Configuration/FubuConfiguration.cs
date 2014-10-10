using Hydra.Queries;

namespace Hydra.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using FubuCore;
    using FubuCore.Binding;
    using FubuCore.Binding.InMemory;
    using FubuCore.Binding.Values;
    using FubuMVC.Core.Http.AspNet;
    using FubuMVC.Core.Runtime;
    using FubuMVC.Core.UI;
    using FubuMVC.Core.UI.Elements;
    using FubuMVC.Core.UI.Security;
    using HtmlTags.Conventions;
    using Hydra.FubuConventions;
    using Microsoft.Practices.Unity;

    public class FubuConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            IMediator mediator = new LazyMediator(() => this.Container.Resolve<IMediator>());

            HtmlConventionLibrary htmlConventionLibrary = new HtmlConventionLibrary();
            htmlConventionLibrary.Import(new OverrideHtmlConventions(mediator).Library);

            this.Container.RegisterInstance<HtmlConventionLibrary>(htmlConventionLibrary);

            this.Container.RegisterType<IValueSource, RequestPropertyValueSource>("RequestPropertyValueSource");

            this.Container.RegisterType<ITagRequestActivator, ElementRequestActivator>();
            this.Container.RegisterType<ITagRequestActivator, ElementRequestActivator>("ElementRequestActivator");
            this.Container.RegisterType<ITagRequestActivator, ServiceLocatorTagRequestActivator>("ServiceLocatorTagRequestActivator");

            this.Container.RegisterType<RequestData>(new InjectionConstructor(typeof(IEnumerable<IValueSource>)));

            this.Container.RegisterType<HttpRequestBase, HttpRequestWrapper>();
            this.Container.RegisterType<HttpContextBase, HttpContextWrapper>();

            this.Container.RegisterType<HttpRequest>(new InjectionFactory(_ => HttpContext.Current.Request));
            this.Container.RegisterType<HttpContext>(new InjectionFactory(_ => HttpContext.Current));

            this.Container.RegisterType<ITypeResolverStrategy, TypeResolver.DefaultStrategy>();

            this.Container.RegisterType<IElementNamingConvention, DotNotationElementNamingConvention>();

            this.Container.RegisterType(typeof(ITagGenerator<>), typeof(TagGenerator<>));
            this.Container.RegisterType(typeof(IElementGenerator<>), typeof(ElementGenerator<>));

            this.Container.RegisterType<IServiceLocator, FubuUnityServiceLocator>();

            this.Container.RegisterType<IBindingLogger, NulloBindingLogger>();

            this.Container.RegisterTypes(
                new[]
                    {
                        typeof(IFubuRequest).Assembly, 
                        typeof(ITypeResolver).Assembly, 
                        typeof(ITagGeneratorFactory).Assembly, 
                        typeof(IFieldAccessService).Assembly
                    }.SelectMany(a => a.GetTypes()),
                WithMappings.FromMatchingInterface);
        }

        private class FubuUnityServiceLocator : IServiceLocator
        {
            private readonly IUnityContainer container;

            public FubuUnityServiceLocator(IUnityContainer container)
            {
                Contract.Requires(container != null);

                this.container = container;
            }

            public T GetInstance<T>()
            {
                return this.container.Resolve<T>();
            }

            public object GetInstance(Type type)
            {
                return this.container.Resolve(type);
            }

            public T GetInstance<T>(string name)
            {
                return this.container.Resolve<T>(name);
            }
        }
    }
}