namespace Hydra.Infrastructure.Test.Calendaring
{
    using Hydra.Infrastructure.Calendaring;
    using Hydra.Infrastructure.Test.Utility;
    using Xunit;
    using Xunit.Extensions;

    public class DurationBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_15_Minutes_Before(DurationBuilder sut)
        {
            string actual = sut.Duration(-15.Minutes());

            string expected = "-PT15M";

            Assert.Equal(actual, expected);
        }

        [Theory, ContainerData]
        public void Should_Build_15_Days_5_Hours_20_Seconds(DurationBuilder sut)
        {
            string actual = sut.Duration(15.Days() + 5.Hours() + 20.Seconds());

            string expected = "P15DT5H20S";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_7_Weeks(DurationBuilder sut)
        {
            string actual = sut.Weeks(7);

            string expected = "P7W";
            
            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_7_Weeks_Before(DurationBuilder sut)
        {
            string actual = sut.Weeks(-7);

            string expected = "-P7W";

            Assert.Equal(actual, expected);
        }
    }
}