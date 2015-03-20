namespace Hydra.Infrastructure
{
    using FluentNHibernate.Mapping;

    public abstract class EntityMap<T> : ClassMap<T>
        where T : Entity
    {
        protected EntityMap()
        {
            this.Id(x => x.Id);
            this.ApplyFilter<NotDeletedFilter>();
        }
    }
}