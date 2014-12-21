namespace Hydra.Import.Test
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DateTimeValueWriterTests
    {
        [Fact]
        public void Should_Be_Able_To_Write_German_DateTime()
        {
            string value = "17.12.2014 13:54";

            var instance = new ValueWriterTestObject();

            PropertyInfo property = Property.Get((ValueWriterTestObject x) => x.DateTime);

            IValueWriter sut = new DateTimeWriter(property);

            CultureInfo source = CultureInfo.CreateSpecificCulture("de-DE");

            CultureInfo target = CultureInfo.InvariantCulture;

            sut.Write(instance, value, source, target);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), instance.DateTime);
        }

        [Fact]
        public void Should_Be_Able_To_Write_Us_DateTime()
        {
            string value = "12/17/2014 13:54";

            var instance = new ValueWriterTestObject();

            PropertyInfo property = Property.Get((ValueWriterTestObject x) => x.DateTime);

            IValueWriter sut = new DateTimeWriter(property);

            CultureInfo source = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo target = CultureInfo.InvariantCulture;

            sut.Write(instance, value, source, target);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), instance.DateTime);
        }

        [Fact]
        public void Should_Be_Able_To_Write_Roundtrip_DateTime()
        {
            string value = "2014-12-17T13:54:00.0000000";

            var foo = new ValueWriterTestObject();

            PropertyInfo property = Property.Get((ValueWriterTestObject f) => f.DateTime);

            IValueWriter writer = new DateTimeWriter(property);

            CultureInfo german = CultureInfo.CreateSpecificCulture("de-DE");

            writer.Write(foo, value, german, CultureInfo.InvariantCulture);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), foo.DateTime);
        }
    }
}
