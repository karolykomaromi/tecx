namespace Hydra.Infrastructure.Test
{
    using System.ComponentModel;
    using Xunit;

    public class EnumerationTypeConverterTests
    {
        [Fact]
        public void Should_Use_EnumerationTypeConverter_To_Convert_Classes_Derived_From_Enumeration()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Numbers));

            Assert.IsType<EnumerationTypeConverter<Numbers>>(sut);
        }

        [Fact]
        public void Should_Convert_From_String()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Numbers));

            Numbers actual = (Numbers)sut.ConvertFrom(Numbers.One.Name);

            Assert.Same(Numbers.One, actual);
        }

        [Fact]
        public void Should_Convert_From_Int32()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Numbers));

            Numbers actual = (Numbers)sut.ConvertFrom(Numbers.One.Value);

            Assert.Same(Numbers.One, actual);
        }

        [Fact]
        public void Should_Convert_To_Int32()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Numbers));

            object converted = sut.ConvertTo(Numbers.One, typeof(int));
            Assert.NotNull(converted);

            int actual = (int)converted;
            Assert.Equal(Numbers.One.Value, actual);
        }

        [Fact]
        public void Should_Convert_To_String()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Numbers));

            string actual = (string)sut.ConvertTo(Numbers.One, typeof(string));

            Assert.Same(Numbers.One.Name, actual);
        }
    }
}
