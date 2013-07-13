namespace TecX.Query.Test.Hibernate
{
    using FluentNHibernate.Mapping;

    using TecX.Query.PD;

    public class PrincipalMap : ClassMap<PDPrincipal>
    {
        public PrincipalMap()
        {
            this.Id(x => x.PDO_ID);
            this.Map(x => x.PDO_DELETED);
            this.Map(x => x.PrincipalName);
        }
    }
}