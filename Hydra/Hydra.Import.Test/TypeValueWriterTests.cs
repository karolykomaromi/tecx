namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class TypeValueWriterTests
    {
        [Fact]
        public void Should_Write_Assembly_Qualified_Type_Name()
        {
            string value = "Hydra.Nh.Infrastructure.I18n.ResourceItem, Hydra.Nh";

            IValueWriter writer = new TypeValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Type));

            ValueWriterTestObject instance = new ValueWriterTestObject();

            writer.Write(instance, value, CultureInfo.InvariantCulture, CultureInfo.InvariantCulture);

            Assert.Equal(typeof(ResourceItem), instance.Type);
        }
    }
}
