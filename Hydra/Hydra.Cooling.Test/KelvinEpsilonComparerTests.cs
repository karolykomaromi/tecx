namespace Hydra.Cooling.Test
{
    using Xunit;
    using Xunit.Extensions;

    public class KelvinEpsilonComparerTests
    {
        [Theory]
        [InlineData(0.05, 9.95, 10)]
        [InlineData(0.05, 10, 9.95)]
        [InlineData(0.05, 10, 9.975)]
        [InlineData(0.05, 9.975, 10)]
        [InlineData(0.05, 9.975, 9.975)]
        public void Should_Recognize_As_Equal(double epsilon, double x, double y)
        {
            var sut = new KelvinEpsilonComparer(epsilon.Kelvin());

            Assert.Equal(0, sut.Compare(x.Kelvin(), y.Kelvin()));
        }
        
        [Theory]
        [InlineData(0.05, 10, 9.94)]
        [InlineData(0.05, 10, 9)]
        public void Should_Recognize_As_Greater_Than(double epsilon, double x, double y)
        {
            var sut = new KelvinEpsilonComparer(epsilon.Kelvin());

            Assert.True(sut.Compare(x.Kelvin(), y.Kelvin()) > 0);
        }
        
        [Theory]
        [InlineData(0.05, 9.94, 10)]
        [InlineData(0.05, 9, 10)]
        public void Should_Recognize_As_Less_Than(double epsilon, double x, double y)
        {
            var sut = new KelvinEpsilonComparer(epsilon.Kelvin());

            Assert.True(sut.Compare(x.Kelvin(), y.Kelvin()) < 0);
        }
    }
}