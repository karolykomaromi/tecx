namespace Hydra.TestTools
{
    using Hydra.Infrastructure.Mail;
    using MimeKit;
    using Xunit;
    using Xunit.Extensions;

    public class MailAddressBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_Address_With_Two_Part_Last_Name(MailboxAddressBuilder sut)
        {
            MailboxAddress actual = sut.VictorSenYung();

            Assert.Equal("victor.yung@mail.invalid", actual.Address);
        }
    }
}
