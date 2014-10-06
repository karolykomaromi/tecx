namespace Infrastructure.Server.Test.ListViews.Filters
{
    using Infrastructure.ListViews.Filter;
    using Xunit;

    public class LazyFilterFixture
    {
        [Fact]
        public void Should_Not_Create_Filter_Before_IsMatch_Is_Called()
        {
            bool isValueCreated = false;

            IPropertyFilter filter = new LazyPropertyFilter(() =>
                {
                    isValueCreated = true;
                    return new PropertyFilter("Foo");
                });

            Assert.False(isValueCreated);

            filter.IsMatch("Bar");

            Assert.True(isValueCreated);
        }
    }
}
