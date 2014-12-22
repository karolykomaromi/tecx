namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DoubleValueWriterTests
    {
        [Fact]
        public void Should_Write_German_Dobule()
        {
            CultureInfo source = Cultures.GermanGermany;

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DoubleValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Double));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456, instance.Double);
        }

        [Fact]
        public void Should_Write_English_Dobule()
        {
            CultureInfo source = Cultures.EnglishUnitedStates;

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DoubleValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Double));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456, instance.Double);
        }
    }
}