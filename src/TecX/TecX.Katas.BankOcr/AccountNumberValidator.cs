namespace TecX.Katas.BankOcr
{
    using System.Globalization;
    using System.Linq;

    public class AccountNumberValidator
    {
        public int[] GetDigits(int number)
        {
            int length = number.ToString(CultureInfo.InvariantCulture).Length;

            int[] digits = new int[length];

            for (int i = 0; i < length; i++)
            {
                digits[i] = number % 10;
                number = number / 10;
            }

            return digits.Reverse().ToArray();
        }

        public bool IsCorrectChecksum(int checksum)
        {
            return checksum % 11 == 0;
        }

        public int CalculateChecksum(int[] digits)
        {
            int[] digitsInReverseOrder = digits.Reverse().ToArray();

            int checksum = 0;

            for (int i = 0; i < 9; i++)
            {
                checksum += (digitsInReverseOrder[i] * (i + 1));
            }

            return checksum;
        }

        public bool IsValidAccountNumber(int accountNumber)
        {
            int[] digits = this.GetDigits(accountNumber);

            int checksum = this.CalculateChecksum(digits);

            bool isValid = this.IsCorrectChecksum(checksum);

            return isValid;
        }
    }
}