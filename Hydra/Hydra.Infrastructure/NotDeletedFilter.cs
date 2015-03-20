namespace Hydra.Infrastructure
{
    using FluentNHibernate.Mapping;
    using Hydra.Infrastructure.Reflection;

    public class NotDeletedFilter : FilterDefinition
    {
        public NotDeletedFilter()
        {
            this.WithName(typeof(NotDeletedFilter).Name)
                .WithCondition(string.Format("{0} IS NULL", TypeHelper.GetPropertyName((Entity e) => e.DeletedAt)));
        }
    }
}