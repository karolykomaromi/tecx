using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Import.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DecimalValueWriterTests
    {
        [Fact]
        public void Should_Write_German_Decimal()
        {
            CultureInfo source = CultureInfo.CreateSpecificCulture("de-DE");

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DecimalValueWriter(Property.Get((ValueWriterTestObject x) => x.Decimal));

            string value = "123,456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456m, instance.Decimal);
        }

        [Fact]
        public void Should_Write_English_Decimal()
        {
            CultureInfo source = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo target = CultureInfo.InvariantCulture;

            IValueWriter sut = new DecimalValueWriter(Property.Get((ValueWriterTestObject x) => x.Decimal));

            string value = "123.456";

            var instance = new ValueWriterTestObject();

            sut.Write(instance, value, source, target);

            Assert.Equal(123.456m, instance.Decimal);
        }
    }

    public class ValueWriterTestObject
    {
        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public float Float { get; set; }

        public DateTime DateTime { get; set; }
    }
}
