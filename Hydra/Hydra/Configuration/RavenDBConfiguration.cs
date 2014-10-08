namespace Hydra.Configuration
{
    using Microsoft.Practices.Unity;
    using Raven.Client;
    using Raven.Client.Embedded;
    using Raven.Database.Server;

    public class RavenDBConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            // one document store per application
            this.Container.RegisterType<IDocumentStore>(
                new ContainerControlledLifetimeManager(), 
                new InjectionFactory(_ =>
                    {
                        NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);

                        IDocumentStore documentStore = new EmbeddableDocumentStore
                            {
                                RunInMemory = true
                                ////DataDirectory = "App_Data",
                                ////UseEmbeddedHttpServer = true
                            };

                        documentStore = documentStore.Initialize();

                        return documentStore;
                    }));

            // one session per request
            this.Container.RegisterType<IDocumentSession>(
                new HierarchicalLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<IDocumentStore>().OpenSession()));
        }
    }
}