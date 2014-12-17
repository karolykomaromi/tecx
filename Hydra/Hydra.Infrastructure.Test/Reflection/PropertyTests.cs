namespace Hydra.Infrastructure.Test.Reflection
{
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class PropertyTests
    {
        private string Foo { get; set; }

        [Fact]
        public void Should_Get_Property_By_Expression()
        {
            PropertyInfo property = Property.Get(() => this.Foo);

            Assert.Equal("Foo", property.Name);
        }

        [Fact]
        public void Should_Get_Property_From_Instance_By_Expression()
        {
            PropertyInfo property = Property.Get((PropertyTests t) => t.Foo);

            Assert.Equal("Foo", property.Name);
        }
    }
}
