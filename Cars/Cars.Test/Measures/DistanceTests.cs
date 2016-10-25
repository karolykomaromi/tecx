using System;
using FsCheck.Xunit;

namespace Cars.Test.Measures
{
    using Cars.Measures;
    using Xunit;

    public class DistanceTests
    {
        [Property]
        public void Should_Properly_Add(decimal x, decimal y)
        {
            try
            {
                Distance expected = x.Kilometers() + y.Kilometers();
                Distance actual = (x + y).Kilometers();

                Assert.Equal(expected, actual);
            }
            catch (OverflowException)
            {
            }
        }

        [Fact]
        public void Should_Throw_On_Overflow()
        {
            Assert.Throws<OverflowException>(() => decimal.MaxValue.Kilometers() + 10.0.Kilometers());
            Assert.Throws<OverflowException>(() => decimal.MinValue.Kilometers() - 10.0.Kilometers());
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1.5, 2.7, 4.2)]
        public void Should_Add(double x, double y, double expected)
        {
            Assert.Equal(expected.Kilometers(), x.Kilometers() + y.Kilometers());
            Assert.Equal(expected.Meters(), x.Meters() + y.Meters());
            Assert.Equal(expected.Centimeters(), x.Centimeters() + y.Centimeters());
            Assert.Equal(expected.Millimeters(), x.Millimeters() + y.Millimeters());
        }

        [Theory]
        [InlineData(1, 2, -1)]
        [InlineData(2.7, 1.5, 1.2)]
        public void Should_Subtract(double x, double y, double expected)
        {
            Assert.Equal(expected.Kilometers(), x.Kilometers() - y.Kilometers());
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(1.9)]
        [InlineData(-1.9)]
        public void Should_Be_Equal(double x)
        {
            Assert.Equal(x.Kilometers(), x.Kilometers());
            Assert.True(x.Kilometers() == x.Kilometers());
            Assert.Equal(x.Meters(), x.Meters());
            Assert.True(x.Meters() == x.Meters());
            Assert.Equal(x.Centimeters(), x.Centimeters());
            Assert.True(x.Centimeters() == x.Centimeters());
            Assert.Equal(x.Millimeters(), x.Millimeters());
            Assert.True(x.Millimeters() == x.Millimeters());
        }

        [Theory]
        [InlineData(1, 1.1)]
        [InlineData(-1, 0.1)]
        [InlineData(1.9, 2)]
        [InlineData(-1.9, 0)]
        public void Should_Be_Less_Than(double x, double y)
        {
            Assert.True(x.Kilometers() < y.Kilometers());
            Assert.True(x.Meters() < y.Meters());
            Assert.True(x.Centimeters() < y.Centimeters());
            Assert.True(x.Millimeters() < y.Millimeters());
        }

        [Theory]
        [InlineData(1.1, 1)]
        [InlineData(0.1, -1)]
        [InlineData(2, 1.9)]
        [InlineData(0, -1.9)]
        public void Should_Be_Greater_Than(double x, double y)
        {
            Assert.True(x.Kilometers() > y.Kilometers());
            Assert.True(x.Meters() > y.Meters());
            Assert.True(x.Centimeters() > y.Centimeters());
            Assert.True(x.Millimeters() > y.Millimeters());
        }

        [Theory]
        [InlineData(1, 1.1)]
        [InlineData(-1, 0.1)]
        [InlineData(1.9, 2)]
        [InlineData(-1.9, 0)]
        [InlineData(-1.9, -1.9)]
        [InlineData(0, 0)]
        public void Should_Be_Less_Than_Or_Equal(double x, double y)
        {
            Assert.True(x.Kilometers() <= y.Kilometers());
            Assert.True(x.Meters() <= y.Meters());
            Assert.True(x.Centimeters() <= y.Centimeters());
            Assert.True(x.Millimeters() <= y.Millimeters());
        }

        [Theory]
        [InlineData(1.1, 1)]
        [InlineData(0.1, -1)]
        [InlineData(2, 1.9)]
        [InlineData(0, -1.9)]
        [InlineData(-1.9, -1.9)]
        [InlineData(0, 0)]
        public void Should_Be_Greater_Than_Or_Equal(double x, double y)
        {
            Assert.True(x.Kilometers() >= y.Kilometers());
            Assert.True(x.Meters() >= y.Meters());
            Assert.True(x.Centimeters() >= y.Centimeters());
            Assert.True(x.Millimeters() >= y.Millimeters());
        }
    }
}
