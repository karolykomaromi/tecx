namespace TecX.Playground.QueryAbstractionLayer.PD
{
    using TecX.Playground.QueryAbstractionLayer.Filters;

    public class PDIteratorOperator
    {
        public PDIteratorOperator()
        {
            this.PrincipalFilter = PrincipalFilter.Enabled;
            this.IncludeDeletedItems = DeletedItemsFilter.Exclude;
        }

        public PrincipalFilter PrincipalFilter { get; set; }

        public DeletedItemsFilter IncludeDeletedItems { get; set; }
    }
}