namespace TecX.Query.Test.Hibernate
{
    using FluentNHibernate.Mapping;

    using TecX.Query.PD;

    public abstract class PersistentObjectMap<T> : ClassMap<T>
        where T : PersistentObject
    {
        protected PersistentObjectMap()
        {
            this.Id(x => x.PDO_ID);
            this.Map(x => x.PDO_DELETED);

            this.ApplyFilter<DynamicFilter<T>>();
        }
    }
}
