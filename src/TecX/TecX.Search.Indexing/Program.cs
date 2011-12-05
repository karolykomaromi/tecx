namespace TecX.Search.Indexing
{
    using System.Configuration;

    using TecX.Common.Extensions.Primitives;
    using TecX.Search.Data.EF;

    using Topshelf;

    public class Program
    {
        public static void Main(string[] args)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings["MessageSearch"];

            string connectionString = css != null
                                          ? css.ConnectionString
                                          : @"Data Source=.;Initial Catalog=MessageStore;Integrated Security=SSPI;";

            var context = new MessageEntities(connectionString);

            var repository = new EFMessageRepository(context, new FullTextSearchTermProcessor());

            HostFactory.Run(x =>
                {
                    x.Service<FullTextIndexingService>(
                        s =>
                            {
                                s.ConstructUsing(() => new FullTextIndexingService(1.Minutes(), repository));
                                s.WhenStarted(idx => idx.Start());
                                s.WhenStopped(idx => idx.Stop());
                                s.SetServiceName("idxSvc");
                            });
                    x.SetDescription("Runs indexing for full-text search.");
                    x.SetDisplayName("Messages full-text index service");
                    x.SetServiceName("idxSvc");
                    x.RunAsLocalSystem();
                });
        }
    }
}
