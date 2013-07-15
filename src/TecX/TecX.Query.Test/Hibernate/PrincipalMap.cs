namespace TecX.Query.Test.Hibernate
{
    using TecX.Query.PD;

    public class PrincipalMap : HistorizableMap<PDPrincipal>
    {
        public PrincipalMap()
        {
            this.Map(x => x.PrincipalName);
        }
    }
}