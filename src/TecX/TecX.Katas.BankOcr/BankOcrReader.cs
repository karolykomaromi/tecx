namespace TecX.Katas.BankOcr
{
    using System;
    using System.Linq;

    public class BankOcrReader
    {
        public int Parse(string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var ocrDigits = lines[0].Zip(lines[1], (x, y) => new string(new[] { x, y })).Zip(lines[2], (x, y) => x + y);

            int number = 0;

            foreach (var chunk in ocrDigits.Chunk(3))
            {
                string digit = string.Join(string.Empty, chunk);

                if (string.Equals(digit, " ||_ _ ||"))
                {
                    number = number * 10 + 0;
                }
                else if (string.Equals(digit, "       ||"))
                {
                    number = number * 10 + 1;
                }
                else if (string.Equals(digit, "  |___ | "))
                {
                    number = number * 10 + 2;
                }
                else if (string.Equals(digit, "   ___ ||"))
                {
                    number = number * 10 + 3;
                }
                else if (string.Equals(digit, " |  _  ||"))
                {
                    number = number * 10 + 4;
                }
                else if (string.Equals(digit, " | ___  |"))
                {
                    number = number * 10 + 5;
                }
                else if (string.Equals(digit, " ||___  |"))
                {
                    number = number * 10 + 6;
                }
                else if (string.Equals(digit, "   _   ||"))
                {
                    number = number * 10 + 7;
                }
                else if (string.Equals(digit, " ||___ ||"))
                {
                    number = number * 10 + 8;
                }
                else if (string.Equals(digit, " | ___ ||"))
                {
                    number = number * 10 + 9;
                }
            }

            return number;
        }
    }
}