namespace Hydra.Infrastructure.Test.I18n
{
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Test.Reflection;
    using Xunit;

    public class ResourceAccessorCacheTests
    {
        [Fact]
        public void Should_Return_Accessor_For_Existing_Resource()
        {
            var sut = new ResourceAccessorCache();

            Assert.Equal("My Internationalized Property Name", sut.GetAccessor(typeof(Foo), "Bar")());
        }

        [Fact]
        public void Should_Return_Cached_Accessor_On_Subsequent_Calls()
        {
            var sut = new ResourceAccessorCache();

            var a1 = sut.GetAccessor(typeof(Foo), "Bar");
            var a2 = sut.GetAccessor(typeof(Foo), "Bar");

            Assert.Same(a1, a2);
        }
    }
}