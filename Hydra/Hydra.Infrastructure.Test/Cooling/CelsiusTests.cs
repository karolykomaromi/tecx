namespace Hydra.Infrastructure.Test.Cooling
{
    using Hydra.Infrastructure.Cooling;
    using Xunit;
    using Xunit.Extensions;

    public class CelsiusTests
    {
        [Theory]
        [InlineData(2.5, 3.7, 6.2)]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 7.5, -2.5)]
        public void Should_Correctly_Add_Values(double x, double y, double z)
        {
            Celsius actual = x.DegreesCelsius() + y.DegreesCelsius();

            Celsius expected = z.DegreesCelsius();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2.5, 3.7, -1.2)]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 7.5, -17.5)]
        public void Should_Correctly_Subtract_Values(double x, double y, double z)
        {
            Celsius actual = x.DegreesCelsius() - y.DegreesCelsius();

            Celsius expected = z.DegreesCelsius();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3.5, 4.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        public void Should_Correctly_Identify_Lesser_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(x < y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3.5, 4.5)]
        [InlineData(-3.5, -3.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        [InlineData(-4.5, -4.5)]
        public void Should_Correctly_Identify_Lesser_Or_Equal_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(x <= y);
        }

        [Theory]
        [InlineData(3.5, 4.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        public void Should_Correctly_Identify_Greater_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(y > x);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3.5, 4.5)]
        [InlineData(-3.5, -3.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        [InlineData(-4.5, -4.5)]
        public void Should_Correctly_Identify_Greater_Or_Equal_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(y >= x);
        }

        [Fact]
        public void Any_Object_Compares_Greater_Than_Null()
        {
            Celsius a = new Celsius(1);

            Assert.True(a.CompareTo(null) > 0);
        }

        [Fact]
        public void Compare_To_Self_Returns_Zero()
        {
            Celsius a = new Celsius(1);

            Assert.Equal(0, a.CompareTo(a));
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Zero_Then_B_CompareTo_A_Must_Return_Zero()
        {
            Celsius a = new Celsius(1);
            Celsius b = new Celsius(1);

            Assert.Equal(0, a.CompareTo(b));
            Assert.Equal(0, b.CompareTo(a));
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Zero_And_B_CompareTo_C_Returns_Zero_Then_A_CompareTo_C_Must_Return_Zero()
        {
            Celsius a = new Celsius(1);
            Celsius b = new Celsius(1);
            Celsius c = new Celsius(1);

            Assert.Equal(0, a.CompareTo(b));
            Assert.Equal(0, b.CompareTo(c));
            Assert.Equal(0, a.CompareTo(c));
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Non_Zero_B_CompareTo_A_Must_Return_Value_Of_Opposite_Sign()
        {
            Celsius a = new Celsius(10);
            Celsius b = new Celsius(20);

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Non_Zero_And_B_CompareTo_C_Returns_Value_Of_Same_Sign_Then_A_CompareTo_C_Must_Return_Value_Of_Same_Sign()
        {
            Celsius a = new Celsius(10);
            Celsius b = new Celsius(20);
            Celsius c = new Celsius(30);

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(c) < 0);
            Assert.True(a.CompareTo(c) < 0);

            Assert.True(c.CompareTo(b) > 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.True(c.CompareTo(a) > 0);
        }

        [Fact]
        public void Should_Equal_To_Self()
        {
            Celsius c = new Celsius(10);

            Assert.True(c.Equals(c));
        }

        [Fact]
        public void Should_Not_Equal_Null()
        {
            Celsius c = new Celsius(10);

            Assert.False(c.Equals(null));
        }

        [Fact]
        public void Same_Values_Should_Equal()
        {
            Celsius x = new Celsius(10);
            Celsius y = new Celsius(10);

            Assert.True(x == y);
        }
    }
}
