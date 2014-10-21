namespace Hydra.Infrastructure.Test
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Xunit;

    public class ConvertHelperTests
    {
        [Fact]
        public void Should_Return_False_If_Conversion_Not_Possible()
        {
            object converted;
            Assert.False(ConvertHelper.TryConvert(new D(), typeof(string), CultureInfo.CreateSpecificCulture("de-DE"), out converted));
        }

        [Fact]
        public void Should_Throw_If_Conversion_Not_Possible()
        {
            Assert.Throws<NotSupportedException>(() => ConvertHelper.Convert(new D(), typeof(string), CultureInfo.CreateSpecificCulture("de-DE")));
        }

        [Fact]
        public void From()
        {
            DateTime now = TimeProvider.UtcNow;

            string s = now.ToString("o");

            C actual = (C)ConvertHelper.Convert(s, typeof(C), CultureInfo.CreateSpecificCulture("de-DE"));

            Assert.Equal(now, actual.Timestamp);
        }

        [Fact]
        public void Should_Convert_To()
        {
            DateTime now = TimeProvider.UtcNow;

            A f = new A { Timestamp = now };

            B actual = (B)ConvertHelper.Convert(f, typeof(B), CultureInfo.CreateSpecificCulture("de-DE"));

            Assert.Equal(f.Timestamp, actual.Timestamp);
        }

        [TypeConverter(typeof(CanConvertFromA2B))]
        private class A
        {
            public DateTime Timestamp { get; set; }
        }

        private class B
        {
            public DateTime Timestamp { get; set; }
        }

        [TypeConverter(typeof(CanConvertFromString2C))]
        private class C
        {
            public DateTime Timestamp { get; set; }
        }

        [TypeConverter(typeof(CantConvertAtAll))]
        private class D
        {
        }

        private class CanConvertFromA2B : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return true;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                A from = (A)value;

                return new B { Timestamp = from.Timestamp };
            }
        }

        private class CanConvertFromString2C : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type destinationType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var converted = new C { Timestamp = DateTime.ParseExact((string)value, "o", null, DateTimeStyles.RoundtripKind) };

                return converted;
            }
        }

        private class CantConvertAtAll : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return false;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return false;
            }
        }
    }
}
