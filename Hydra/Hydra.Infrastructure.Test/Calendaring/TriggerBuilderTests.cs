namespace Hydra.Infrastructure.Test.Calendaring
{
    using System;
    using Hydra.Infrastructure.Calendaring;
    using Xunit;
    using Xunit.Extensions;

    public class TriggerBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_5_Minutes_After_End(TriggerBuilder sut)
        {
            string actual = sut.FromDuration(x => x.Duration(5.Minutes())).AfterEnd().Build();

            string expected = "TRIGGER;RELATED=END:PT5M";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Absolute(TriggerBuilder sut)
        {
            string actual = sut.Absolute(new DateTime(1998, 1, 1, 5, 0, 0)).Build();

            string expected = "TRIGGER;VALUE=DATE-TIME:19980101T050000Z";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_15_Minutes_Before_Start(TriggerBuilder sut)
        {
            string actual = sut.FromDuration(x => x.Duration(-15.Minutes())).Build();

            string expected = "TRIGGER:-PT15M";

            Assert.Equal(expected, actual);
        }
    }
}