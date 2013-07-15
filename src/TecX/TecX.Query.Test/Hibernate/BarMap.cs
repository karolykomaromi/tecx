namespace TecX.Query.Test.Hibernate
{
    using TecX.Query.PD;

    public class BarMap : PDObjectMap<Bar>
    {
        public BarMap()
        {
            this.Map(x => x.Description);
        }
    }
}