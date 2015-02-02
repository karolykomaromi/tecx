namespace Hydra.Infrastructure.Test
{
    using System.ComponentModel;
    using Xunit;

    public class FlagsTypeConverterTests
    {
        [Fact]
        public void Should_Use_FlagsTypeConverter_To_Convert_Classes_Derived_From_Flags()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Colors));

            Assert.IsType<FlagsTypeConverter<Colors>>(sut);
        }

        [Fact]
        public void Should_Convert_Single_Value_From_String()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Colors));

            Colors actual = (Colors)sut.ConvertFrom(Colors.Blue.Name);

            Assert.Same(Colors.Blue, actual);
        }

        [Fact]
        public void Should_Convert_Multiple_Values_From_String()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Colors));

            Colors actual = (Colors)sut.ConvertFrom(Colors.Red.Name + "," + Colors.Blue.Name);

            Assert.Equal(Colors.Blue | Colors.Red, actual);
        }

        [Fact]
        public void Should_Convert_Single_Value_From_Int32()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Colors));

            Colors actual = (Colors)sut.ConvertFrom(Colors.Red.Value);

            Assert.Equal(Colors.Red, actual);
        }

        [Fact]
        public void Should_Convert_Number_Zero_To_Default()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Colors));

            Colors actual = (Colors)sut.ConvertFrom(0);

            Assert.Equal(Colors.Default, actual);
        }

        [Fact]
        public void Should_Convert_Multiple_Values_From_Int32()
        {
            TypeConverter sut = TypeDescriptor.GetConverter(typeof(Colors));

            Colors actual = (Colors)sut.ConvertFrom(Colors.Red.Value + Colors.Blue.Value);

            Assert.Equal(Colors.Blue | Colors.Red, actual);
        }
    }
}