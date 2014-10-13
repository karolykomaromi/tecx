namespace Hydra.Infrastructure.Test.I18n
{
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Test.Reflection;
    using Hydra.Infrastructure.Test.Utility;
    using Xunit;
    using Xunit.Extensions;

    public class ResourceAccessorCacheTests
    {
        [Theory, ContainerData]
        public void Should_Return_Accessor_For_Existing_Resource(ResourceAccessorCache sut)
        {
            Assert.Equal("My Internationalized Property Name", sut.GetAccessor(typeof(Foo), "Bar")());
        }

        [Theory, ContainerData]
        public void Should_Return_Cached_Accessor_On_Subsequent_Calls(ResourceAccessorCache sut)
        {
            var a1 = sut.GetAccessor(typeof(Foo), "Bar");
            var a2 = sut.GetAccessor(typeof(Foo), "Bar");

            Assert.Same(a1, a2);
        }

        [Theory, ContainerData]
        public void Should_Return_Accessor_For_Type_Full_Named_Resource(ResourceAccessorCache sut)
        {
            Assert.Equal("I'm a full type named resource.", sut.GetAccessor(typeof(Foo), "FullTypeNamedResourceExpected")());
        }
    }
}