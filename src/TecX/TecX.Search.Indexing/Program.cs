namespace TecX.Search.Indexing
{
    using TecX.Common.Extensions.Primitives;

    using Topshelf;

    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
                {
                    x.Service<FullTextIndexingService>(
                        s =>
                            {
                                s.ConstructUsing(() => new FullTextIndexingService(10.Minutes()));
                                s.WhenStarted(idx => idx.Start());
                                s.WhenStopped(idx => idx.Stop());
                                s.SetServiceName("idxSvc");
                            });
                    x.SetDescription("Runs indexing for full-text search.");
                    x.SetDisplayName("Customers full-text index service");
                    x.SetServiceName("idxSvc");
                    x.RunAsLocalSystem();
                });
        }
    }
}
