namespace Infrastructure.Server.Test.ListViews.Filters
{
    using Infrastructure.ListViews.Filter;
    using Moq;
    using Xunit;

    public class CompositePropertyFilterFixture
    {
        [Fact]
        public void Should_Call_All_Inner_Filters()
        {
            var f1 = new Mock<IPropertyFilter>();
            f1.Setup(f => f.IsMatch(It.IsAny<string>())).Returns(false);
            var f2 = new Mock<IPropertyFilter>();
            f2.Setup(f => f.IsMatch(It.IsAny<string>())).Returns(false);

            IPropertyFilter filter = new CompositeFilter(f1.Object, f2.Object);

            filter.IsMatch("Foo");

            f1.Verify(f => f.IsMatch("Foo"), Times.Once);
            f2.Verify(f => f.IsMatch("Foo"), Times.Once);
        }

        [Fact]
        public void Should_Not_Match_When_Empty()
        {
            IPropertyFilter filter = new CompositeFilter();

            Assert.False(filter.IsMatch("Foo"));
        }
    }
}
