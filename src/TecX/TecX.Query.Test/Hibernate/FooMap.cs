namespace TecX.Query.Test.Hibernate
{
    using TecX.Query.PD;

    public class FooMap : PDObjectMap<Foo>
    {
        public FooMap()
        {
            this.Map(x => x.Description);

            this.HasMany(x => x.Bars)
                    .ApplyFilter<DescriptionFilter>()
                    .ApplyFilter<PrincipalFilter>();
        }
    }
}