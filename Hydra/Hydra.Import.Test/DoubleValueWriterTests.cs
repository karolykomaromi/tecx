namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DoubleValueWriterTests
    {
        [Fact]
        public void Should_Write_German_Dobule()
        {
            CultureInfo source = CultureInfo.CreateSpecificCulture("de-DE");

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DoubleValueWriter(Property.Get((ValueWriterTestObject x) => x.Double));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456, instance.Double);
        }

        [Fact]
        public void Should_Write_English_Dobule()
        {
            CultureInfo source = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DoubleValueWriter(Property.Get((ValueWriterTestObject x) => x.Double));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456, instance.Double);
        }
    }
}