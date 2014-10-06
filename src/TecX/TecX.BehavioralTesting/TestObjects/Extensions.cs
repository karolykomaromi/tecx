namespace TecX.BehavioralTesting.TestObjects
{
    using Xunit;

    public static class Extensions
    {
        public static void ShouldEqual(this double actual, double expected)
        {
            Assert.Equal(expected, actual);
        }
    }
}