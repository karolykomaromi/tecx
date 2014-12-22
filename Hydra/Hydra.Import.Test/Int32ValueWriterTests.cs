namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class Int32ValueWriterTests
    {
        [Fact]
        public void Should_Write_Negative_Value()
        {
            IValueWriter writer = new Int32ValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Int32));

            string value = "-1.000.000";

            CultureInfo source = Cultures.GermanGermany;

            ValueWriterTestObject instance = new ValueWriterTestObject();

            writer.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(-1000000, instance.Int32);
        }

        [Fact]
        public void Should_Write_Value_With_Dot_Thousand_Separator()
        {
            IValueWriter writer = new Int32ValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Int32));

            string value = "1.000.000";

            CultureInfo source = Cultures.GermanGermany;

            ValueWriterTestObject instance = new ValueWriterTestObject();

            writer.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(1000000, instance.Int32);
        }

        [Fact]
        public void Should_Write_Value_With_Conmma_Thousand_Separator()
        {
            IValueWriter writer = new Int32ValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.Int32));

            string value = "1,000,000";

            CultureInfo source = Cultures.EnglishUnitedStates;

            ValueWriterTestObject instance = new ValueWriterTestObject();

            writer.Write(instance, value, source, CultureInfo.InvariantCulture);

            Assert.Equal(1000000L, instance.Int32);
        }
    }
}