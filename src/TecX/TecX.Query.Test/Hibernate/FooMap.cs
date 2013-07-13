namespace TecX.Query.Test.Hibernate
{
    using FluentNHibernate.Mapping;

    using TecX.Query.PD;

    public class FooMap : ClassMap<Foo>
    {
        public FooMap()
        {
            this.Id(x => x.PDO_ID);
            this.Map(x => x.Description);
            this.Map(x => x.PDO_DELETED);

            this.References(x => x.Principal);
            this.HasMany(x => x.Bars).Inverse();
        }
    }
}