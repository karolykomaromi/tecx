namespace Infrastructure.Server.Test.ListViews.Filters
{
    using Infrastructure.ListViews.Filter;
    using Xunit;

    public class PropertyFilterFixture
    {
        [Fact]
        public void Should_Match_IgnoringCase()
        {
            IPropertyFilter filter = new PropertyFilter("FOO");

            Assert.True(filter.IsMatch("foo"));
        }

        [Fact]
        public void Should_Not_Match_Different_PropertyName()
        {
            IPropertyFilter filter = new PropertyFilter("FOO");

            Assert.False(filter.IsMatch("bar"));
        }
    }
}
