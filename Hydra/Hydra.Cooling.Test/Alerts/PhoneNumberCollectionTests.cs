namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Xunit;

    public class PhoneNumberCollectionTests
    {
        [Fact]
        public void Should_Sort_Phone_Numbers()
        {
            PhoneNumber p1 = new PhoneNumber(new CountryCode(41), new AreaCode(123), new DialNumber(4567));
            PhoneNumber p2 = new PhoneNumber(new CountryCode(43), new AreaCode(123), new DialNumber(4567));

            PhoneNumberCollection sut = new PhoneNumberCollection(p2, p1);

            Assert.Same(p1, sut[0]);
            Assert.Same(p2, sut[1]);
        }

        [Fact]
        public void Should_Automatically_Remove_Duplicates()
        {
            PhoneNumber p1 = new PhoneNumber(new CountryCode(41), new AreaCode(123), new DialNumber(4567));
            PhoneNumber p2 = new PhoneNumber(new CountryCode(41), new AreaCode(123), new DialNumber(4567));

            PhoneNumberCollection sut = new PhoneNumberCollection(p1, p2);

            Assert.Equal(1, sut.Count);
        }
    }
}