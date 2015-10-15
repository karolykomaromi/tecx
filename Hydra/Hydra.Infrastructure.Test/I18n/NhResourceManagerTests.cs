namespace Hydra.Infrastructure.Test.I18n
{
    using System.Globalization;
    using System.Linq;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using NHibernate;
    using Xunit;
    using Xunit.Extensions;

    public class NhResourceManagerTests
    {
        [Theory, ContainerData]
        [Trait("Category", "Integration")]
        public void Should_Find_ResourceItem_By_Name_And_Culture(ISession session)
        {
            using (session)
            {
                using (var tx = session.BeginTransaction())
                {
                    CultureInfo culture = Cultures.EnglishUnitedKingdom;

                    ResourceItemBuilder builder =
                        new ResourceItemBuilder().FromType(typeof(Properties.Resources)).UseCulture(culture);

                    ResourceItem r1 = builder.ForProperty(() => Properties.Resources.Foo).WithValue("1");
                    ResourceItem r2 = builder.ForProperty(() => Properties.Resources.Bar).WithValue("2");

                    session.Save(r1);
                    session.Save(r2);

                    IResourceManager sut = new NhResourceManager(typeof(Properties.Resources).FullName, session);

                    Assert.Equal("1", sut.GetString(TypeHelper.GetPropertyName<string>(() => Properties.Resources.Foo), culture));
                    Assert.Equal("2", sut.GetString(TypeHelper.GetPropertyName<string>(() => Properties.Resources.Bar), culture));

                    tx.Rollback();
                }
            }
        }

        [Theory, ContainerData]
        [Trait("Category", "Integration")]
        public void Should_Find_All_ResourceItems_For_Given_Base_Name_And_Culture(ISession session)
        {
            using (session)
            {
                using (var tx = session.BeginTransaction())
                {
                    CultureInfo culture = Cultures.EnglishUnitedKingdom;

                    ResourceItemBuilder builder =
                        new ResourceItemBuilder().FromType(typeof(Properties.Resources)).UseCulture(culture);

                    ResourceItem r1 = builder.ForProperty(() => Properties.Resources.Foo).WithValue("1");
                    ResourceItem r2 = builder.ForProperty(() => Properties.Resources.Bar).WithValue("2");

                    long id1 = (long)session.Save(r1);
                    long id2 = (long)session.Save(r2);

                    var sut = new NhResourceManager(typeof(Properties.Resources).FullName, session);

                    var actual = sut.GetAll(culture).OrderBy(ri => ri.Id).ToArray();

                    Assert.Equal(2, actual.Length);
                    Assert.Equal(id1, actual[0].Id);
                    Assert.Equal(id2, actual[1].Id);

                    tx.Rollback();
                }
            }
        }
    }
}
