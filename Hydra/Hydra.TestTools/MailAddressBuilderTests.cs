namespace Hydra.TestTools
{
    using System.Net.Mail;
    using Xunit;
    using Xunit.Extensions;

    public class MailAddressBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_Address_With_Two_Part_Last_Name(MailAddressBuilder sut)
        {
            MailAddress actual = sut.VictorSenYung();

            Assert.Equal("victor.yung@mail.invalid", actual.Address);
        }
    }
}
