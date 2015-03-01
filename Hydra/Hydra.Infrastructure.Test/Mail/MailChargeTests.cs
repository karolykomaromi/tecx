namespace Hydra.Infrastructure.Test.Mail
{
    using System;
    using Hydra.Infrastructure.Extensions;
    using Hydra.Infrastructure.Mail;
    using Xunit;

    public class MailChargeTests
    {
        private static readonly DateTime ReferenceDate = new DateTime(2015, 3, 1, 17, 37, 36, DateTimeKind.Utc);

        [Fact]
        public void Should_Display_MailCharge_Id_As_Chunks_Of_4_Digits()
        {
            ReferenceDate.Freeze();

            MailCharge charge = MailCharge.NewCharge();

            Assert.Equal("1100-5B42-64B0-00", charge.Id);
        }

        [Fact]
        public void Should_Parse_Correct_MailCharge_Id()
        {
            MailCharge actual;
            Assert.True(MailCharge.TryParse("1100-5B42-64B0-00", out actual));

            Assert.Equal(ReferenceDate, actual.Timestamp);
        }

        [Fact]
        public void Should_Compare_Charges_Correctly()
        {
            ReferenceDate.Freeze();

            MailCharge a = MailCharge.NewCharge();

            5.Minutes().Pass();

            MailCharge b = MailCharge.NewCharge();

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.True(a.CompareTo(a) == 0);
        }

        [Fact]
        public void Should_Recognize_Equal_Charges()
        {
            ReferenceDate.Freeze();

            MailCharge a = MailCharge.NewCharge();

            MailCharge b = MailCharge.NewCharge();

            Assert.Equal(a, b);
        }

        [Fact]
        public void Should_Recognize_Unequal_Charges()
        {
            ReferenceDate.Freeze();

            MailCharge a = MailCharge.NewCharge();

            5.Minutes().Pass();

            MailCharge b = MailCharge.NewCharge();

            Assert.NotEqual(a, b);
        }
    }
}