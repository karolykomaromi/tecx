namespace TecX.Query.Test.Hibernate
{
    using FluentNHibernate.Mapping;

    public class DescriptionFilter : FilterDefinition
    {
        /// <summary>
        /// Description
        /// </summary>
        public const string Description = "Description";

        public DescriptionFilter()
        {
            this.WithName(this.GetType().Name)
                .AddParameter(Description, NHibernate.NHibernateUtil.String)
                .WithCondition(Description + " == :" + Description);
        }
    }

    public class PrincipalFilter : FilterDefinition
    {
        public PrincipalFilter()
        {
            this.WithName(this.GetType().Name)
                .AddParameter(BarMap.ForeignKeyColumns.Principal, NHibernate.NHibernateUtil.Int64)
                .WithCondition(BarMap.ForeignKeyColumns.Principal + " == :" + BarMap.ForeignKeyColumns.Principal);
        }
    }
}