namespace Hydra.Import.Test
{
    using System;
    using System.Linq;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Nh.Infrastructure.I18n;
    using Hydra.Nh.Mappings;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Linq;
    using NHibernate.Tool.hbm2ddl;
    using Xunit;

    public class NhBulkImportWriterTests : IDisposable
    {
        private readonly ISessionFactory sessionFactory;

        private Configuration configuration;

        public NhBulkImportWriterTests()
        {
            this.sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.InMemory())
                    .Mappings(m => m.FluentMappings.Add<ResourceItemMap>())
                    .ExposeConfiguration(
                        cfg =>
                        {
                            this.configuration = cfg;
                        })
                    .BuildSessionFactory();
        }

        [Fact]
        public void Should_Write_To_Db()
        {
            this.RunStatelessWithTx(
                session =>
                {
                    IImportWriter<ResourceItem> writer = new NhBulkImportWriter<ResourceItem>(session);

                    var resx = new[]
                                   {
                                       new ResourceItem { Name = "Foo", TwoLetterISOLanguageName = "DE", Value = "Foo_DE" },
                                       new ResourceItem { Name = "Bar", TwoLetterISOLanguageName = "EN", Value = "Bar_EN" }
                                   };

                    writer.Write(resx);

                    var resourceItems = session.Query<ResourceItem>().ToArray();

                    Assert.Equal(2, resourceItems.Length);
                });
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.sessionFactory.Dispose();
            }
        }

        private void RunStatelessWithTx(Action<IStatelessSession> action)
        {
            using (IStatelessSession session = this.sessionFactory.OpenStatelessSession())
            {
                SchemaExport export = new SchemaExport(this.configuration);

                export.Execute(true, true, false, session.Connection, null);

                using (ITransaction tx = session.BeginTransaction())
                {
                    action(session);

                    tx.Commit();
                }
            }
        }
    }
}
