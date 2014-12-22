namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DecimalValueWriterTests
    {
        [Fact]
        public void Should_Write_German_Decimal()
        {
            CultureInfo source = Cultures.GermanGermany;

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456m, instance.Decimal);
        }

        [Fact]
        public void Should_Write_English_Decimal()
        {
            CultureInfo source = Cultures.EnglishUnitedStates;

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456m, instance.Decimal);
        }
    }
}
