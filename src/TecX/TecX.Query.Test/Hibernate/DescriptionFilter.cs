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
                .AddParameter(Description.ToLower(), NHibernate.NHibernateUtil.String)
                .WithCondition(Description + " = :" + Description.ToLower());
        }
    }
}