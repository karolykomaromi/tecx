namespace BankOcrKata
{
    using Ploeh.AutoFixture.Xunit;

    using TecX.Katas.BankOcr;

    using Xunit;
    using Xunit.Extensions;

    public class AccountNumberValidatorFixture
    {
        [Theory, AutoData]
        public void VerifiesCorrectAccountNumber(AccountNumberValidator sut)
        {
            int accountNumber = 345882865;

            Assert.True(sut.IsValidAccountNumber(accountNumber));
        }

        [Theory, AutoData]
        public void GetsCorrectDigits(AccountNumberValidator sut)
        {
            int number = 345882865;

            int[] actual = sut.GetDigits(number);

            int[] expected = new[] { 3, 4, 5, 8, 8, 2, 8, 6, 5 };

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void CalculatesCorrectChecksum(AccountNumberValidator sut)
        {
            int[] digits = new[] { 3, 4, 5, 8, 8, 2, 8, 6, 5 };

            int actual = sut.CalculateChecksum(digits);

            int expected = 231;

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void VerifiesChecksumCorrect(AccountNumberValidator sut)
        {
            int checksum = 231;

            Assert.True(sut.IsCorrectChecksum(checksum));
        }
    }
}