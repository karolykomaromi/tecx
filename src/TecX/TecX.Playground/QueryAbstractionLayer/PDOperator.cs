namespace TecX.Playground.QueryAbstractionLayer
{
    public class PDOperator
    {
        public PDOperator()
        {
            this.IncludePrincipalFilter = PrincipalFilter.Include;
        }

        public PrincipalFilter IncludePrincipalFilter { get; set; }
    }
}