namespace Hydra.Import.Test.ValueWriters
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.ValueWriters;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DateTimeValueWriterTests
    {
        [Fact]
        public void Should_Be_Able_To_Write_German_DateTime()
        {
            string value = "17.12.2014 13:54";

            var instance = new ValueWriterTestObject();

            PropertyInfo property = TypeHelper.GetProperty((ValueWriterTestObject x) => x.DateTime);

            IValueWriter sut = new DateTimeValueWriter(property);

            CultureInfo source = Cultures.GermanGermany;

            CultureInfo target = CultureInfo.InvariantCulture;

            sut.Write(instance, value, source, target);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), instance.DateTime);
        }

        [Fact]
        public void Should_Be_Able_To_Write_Us_DateTime()
        {
            string value = "12/17/2014 13:54";

            var instance = new ValueWriterTestObject();

            PropertyInfo property = TypeHelper.GetProperty((ValueWriterTestObject x) => x.DateTime);

            IValueWriter sut = new DateTimeValueWriter(property);

            CultureInfo source = Cultures.EnglishUnitedStates;

            CultureInfo target = CultureInfo.InvariantCulture;

            sut.Write(instance, value, source, target);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), instance.DateTime);
        }

        [Fact]
        public void Should_Be_Able_To_Write_Roundtrip_DateTime()
        {
            string value = "2014-12-17T13:54:00.0000000";

            var foo = new ValueWriterTestObject();

            PropertyInfo property = TypeHelper.GetProperty((ValueWriterTestObject f) => f.DateTime);

            IValueWriter writer = new DateTimeValueWriter(property);

            CultureInfo german = Cultures.GermanGermany;

            writer.Write(foo, value, german, CultureInfo.InvariantCulture);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), foo.DateTime);
        }
    }
}
