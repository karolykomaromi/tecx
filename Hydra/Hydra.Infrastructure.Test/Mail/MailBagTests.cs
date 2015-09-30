namespace Hydra.Infrastructure.Test.Mail
{
    using System;
    using Hydra.Infrastructure.Extensions;
    using Hydra.Infrastructure.Mail;
    using Xunit;

    public class MailBagTests
    {
        private static readonly DateTime ReferenceDate = new DateTime(2015, 3, 1, 17, 37, 36, DateTimeKind.Utc);

        [Fact]
        public void Should_Display_MailCharge_Id_As_Chunks_Of_4_Digits()
        {
            ReferenceDate.Freeze();

            MailBag bag = MailBag.NewBag();

            Assert.Equal("1100-5B42-64B0-00", bag.Id);
        }

        [Fact]
        public void Should_Parse_Correct_MailCharge_Id()
        {
            MailBag actual;
            Assert.True(MailBag.TryParse("1100-5B42-64B0-00", out actual));

            Assert.Equal(ReferenceDate, actual.Timestamp);
        }

        [Fact]
        public void Should_Compare_Charges_Correctly()
        {
            ReferenceDate.Freeze();

            MailBag a = MailBag.NewBag();

            5.Minutes().Pass();

            MailBag b = MailBag.NewBag();

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.True(a.CompareTo(a) == 0);
        }

        [Fact]
        public void Should_Recognize_Equal_Charges()
        {
            ReferenceDate.Freeze();

            MailBag a = MailBag.NewBag();

            MailBag b = MailBag.NewBag();

            Assert.Equal(a, b);
        }

        [Fact]
        public void Should_Recognize_Unequal_Charges()
        {
            ReferenceDate.Freeze();

            MailBag a = MailBag.NewBag();

            5.Minutes().Pass();

            MailBag b = MailBag.NewBag();

            Assert.NotEqual(a, b);
        }
    }
}