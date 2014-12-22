namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class FloatValueWriterTests
    {
        [Fact]
        public void Should_Write_German_Float()
        {
            CultureInfo source = Cultures.GermanGermany;

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new FloatValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Float));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456f, instance.Float);
        }

        [Fact]
        public void Should_Write_English_Float()
        {
            CultureInfo source = Cultures.EnglishUnitedStates;

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new FloatValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Float));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456f, instance.Float);
        }
    }
}