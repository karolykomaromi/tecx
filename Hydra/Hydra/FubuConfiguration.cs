namespace Hydra
{
    using System.Collections.Generic;
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
    using Microsoft.Practices.Unity;

    public class FubuConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            HtmlConventionLibrary htmlConventionLibrary = new HtmlConventionLibrary();
            htmlConventionLibrary.Import(new DefaultHtmlConventions().Library);

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

            this.Container.RegisterType<IServiceLocator, InMemoryServiceLocator>();
            this.Container.RegisterType<IBindingLogger, RecordingBindingLogger>();

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
    }
}