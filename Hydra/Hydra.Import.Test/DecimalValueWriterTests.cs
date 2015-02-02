namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DecimalValueWriterTests
    {
        [Fact]
        public void Should_Write_Negative_Value()
        {
            CultureInfo source = Cultures.GermanGermany;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "-123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(-123.456m, instance.Decimal);
        }
        
        [Fact]
        public void Should_Write_Value_With_Comma_Decimal_Separator()
        {
            CultureInfo source = Cultures.GermanGermany;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(123.456m, instance.Decimal);
        }

        [Fact]
        public void Should_Write_Value_With_Dot_Decimal_Separator()
        {
            CultureInfo source = Cultures.EnglishUnitedStates;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(123.456m, instance.Decimal);
        }

        [Fact]
        public void Should_Write_Value_With_Dot_Thousand_Separator()
        {
            CultureInfo source = Cultures.GermanGermany;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(123456m, instance.Decimal);
        }

        [Fact]
        public void Should_Write_Value_With_Comma_Thousand_Separator()
        {
            CultureInfo source = Cultures.EnglishUnitedStates;

            IValueWriter sut = new DecimalValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Decimal));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(123456m, instance.Decimal);
        }
    }
}
