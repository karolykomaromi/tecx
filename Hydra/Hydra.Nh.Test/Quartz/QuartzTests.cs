namespace Hydra.Nh.Test.Quartz
{
    using Hydra.Nh.Quartz;
    using NHibernate;
    using NHibernate.Linq;
    using NHibernate.Util;
    using Xunit;
    using Xunit.Extensions;

    public class QuartzTests
    {
        [Theory, ContainerData]
        public void Should_Create_Quartz_Tables(ISession session)
        {
            using (session)
            {
                using (var tx = session.BeginTransaction())
                {
                    Assert.False(session.Query<QuartzSimplePropertiesForTriggers>().Any());

                    tx.Commit();
                }
            }
        }
    }
}
