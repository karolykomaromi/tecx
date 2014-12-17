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
            string s = "17.12.2014 13:54";

            var foo = new Foo();

            PropertyInfo property = Property.Get((Foo f) => f.Timestamp);

            IValueWriter writer = new DateTimeWriter(property);

            CultureInfo german = CultureInfo.CreateSpecificCulture("de-DE");

            writer.Write(foo, s, german, CultureInfo.InvariantCulture);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), foo.Timestamp);
        }

        [Fact]
        public void Should_Be_Able_To_Write_Us_DateTime()
        {
            string s = "12/17/2014 13:54";

            var foo = new Foo();

            PropertyInfo property = Property.Get((Foo f) => f.Timestamp);

            IValueWriter writer = new DateTimeWriter(property);

            CultureInfo us = CultureInfo.CreateSpecificCulture("en-US");

            writer.Write(foo, s, us, CultureInfo.InvariantCulture);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), foo.Timestamp);
        }

        [Fact]
        public void Should_Be_Able_To_Write_Roundtrip_DateTime()
        {
            string s = "2014-12-17T13:54:00.0000000";

            var foo = new Foo();

            PropertyInfo property = Property.Get((Foo f) => f.Timestamp);

            IValueWriter writer = new DateTimeWriter(property);

            CultureInfo german = CultureInfo.CreateSpecificCulture("de-DE");

            writer.Write(foo, s, german, CultureInfo.InvariantCulture);

            Assert.Equal(new DateTime(2014, 12, 17, 13, 54, 0), foo.Timestamp);
        }
    }

    public class Foo
    {
        public DateTime Timestamp { get; set; }
    }
}
