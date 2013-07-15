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
        /// <summary>
        /// PrincipalId
        /// </summary>
        public const string PrincipalId = "PrincipalId";

        public PrincipalFilter()
        {
            this.WithName(this.GetType().Name)
                .AddParameter(PrincipalId, NHibernate.NHibernateUtil.Int64)
                .WithCondition("Principal.PDO_ID == :" + PrincipalId);
        }
    }
}