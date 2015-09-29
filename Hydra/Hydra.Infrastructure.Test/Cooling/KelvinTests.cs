namespace Hydra.Infrastructure.Test.Cooling
{
    using Hydra.Infrastructure.Cooling;
    using Xunit;
    using Xunit.Extensions;

    public class KelvinTests
    {
        [Theory]
        [InlineData(2.5, 3.7, 6.2)]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 7.5, -2.5)]
        public void Should_Correctly_Add_Values(double x, double y, double z)
        {
            Kelvin actual = x.Kelvin() + y.Kelvin();

            Kelvin expected = z.Kelvin();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2.5, 3.7, -1.2)]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 7.5, -17.5)]
        public void Should_Correctly_Subtract_Values(double x, double y, double z)
        {
            Kelvin actual = x.Kelvin() - y.Kelvin();

            Kelvin expected = z.Kelvin();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3.5, 4.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        public void Should_Correctly_Identify_Lesser_Temperature(double lesser, double greater)
        {
            Kelvin x = lesser.Kelvin();
            Kelvin y = greater.Kelvin();

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
            Kelvin x = lesser.Kelvin();
            Kelvin y = greater.Kelvin();

            Assert.True(x <= y);
        }

        [Theory]
        [InlineData(3.5, 4.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        public void Should_Correctly_Identify_Greater_Temperature(double lesser, double greater)
        {
            Kelvin x = lesser.Kelvin();
            Kelvin y = greater.Kelvin();

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
            Kelvin x = lesser.Kelvin();
            Kelvin y = greater.Kelvin();

            Assert.True(y >= x);
        }

        [Fact]
        public void Any_Object_Compares_Greater_Than_Null()
        {
            Kelvin a = new Kelvin(1);

            Assert.True(a.CompareTo(null) > 0);
        }

        [Fact]
        public void Compare_To_Self_Returns_Zero()
        {
            Kelvin a = new Kelvin(1);

            Assert.Equal(0, a.CompareTo(a));
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Zero_Then_B_CompareTo_A_Must_Return_Zero()
        {
            Kelvin a = new Kelvin(1);
            Kelvin b = new Kelvin(1);

            Assert.Equal(0, a.CompareTo(b));
            Assert.Equal(0, b.CompareTo(a));
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Zero_And_B_CompareTo_C_Returns_Zero_Then_A_CompareTo_C_Must_Return_Zero()
        {
            Kelvin a = new Kelvin(1);
            Kelvin b = new Kelvin(1);
            Kelvin c = new Kelvin(1);

            Assert.Equal(0, a.CompareTo(b));
            Assert.Equal(0, b.CompareTo(c));
            Assert.Equal(0, a.CompareTo(c));
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Non_Zero_B_CompareTo_A_Must_Return_Value_Of_Opposite_Sign()
        {
            Kelvin a = new Kelvin(10);
            Kelvin b = new Kelvin(20);

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
        }

        [Fact]
        public void If_A_CompareTo_B_Returns_Non_Zero_And_B_CompareTo_C_Returns_Value_Of_Same_Sign_Then_A_CompareTo_C_Must_Return_Value_Of_Same_Sign()
        {
            Kelvin a = new Kelvin(10);
            Kelvin b = new Kelvin(20);
            Kelvin c = new Kelvin(30);

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(c) < 0);
            Assert.True(a.CompareTo(c) < 0);

            Assert.True(c.CompareTo(b) > 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.True(c.CompareTo(a) > 0);
        }
    }
}