﻿namespace Hydra.Import.Test
{
    using System.Linq;
    using Hydra.Nh.Infrastructure.I18n;
    using NHibernate;
    using NHibernate.Linq;
    using Xunit;
    using Xunit.Extensions;

    public class NhBulkDataWriterTests
    {
        [Theory, ContainerData]
        [Trait("Category", "Integration")]
        public void Should_Write_To_Db(IStatelessSession session)
        {
            using (session)
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    IDataWriter<ResourceItem> writer = new NhBulkDataWriter<ResourceItem>(session);

                    var resx = new[]
                    {
                        new ResourceItem { Name = "Foo", TwoLetterISOLanguageName = "DE", Value = "Foo_DE" },
                        new ResourceItem { Name = "Bar", TwoLetterISOLanguageName = "EN", Value = "Bar_EN" }
                    };

                    writer.Write(resx);

                    var resourceItems = session.Query<ResourceItem>().ToArray();

                    Assert.Equal(2, resourceItems.Length);

                    tx.Commit();
                }
            }
        }
    }
}