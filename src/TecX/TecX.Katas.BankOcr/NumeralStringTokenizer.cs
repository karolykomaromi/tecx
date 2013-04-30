namespace TecX.Katas.BankOcr
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class NumeralStringTokenizer : IEnumerable<string>
    {
        private readonly TextReader reader;

        public NumeralStringTokenizer(TextReader reader)
        {
            this.reader = reader;
        }

        public IEnumerator<string> GetEnumerator()
        {
            string line1 = this.reader.ReadLine();

            AssertLength(line1);

            string line2 = this.reader.ReadLine();

            AssertLength(line2);

            string line3 = this.reader.ReadLine();

            AssertLength(line3);

            string shouldBeEmptyLine = this.reader.ReadLine();

            if (!string.IsNullOrEmpty(shouldBeEmptyLine))
            {
                throw new FormatException("OCR data must be terminated with an empty line or EOF.");
            }

            for (int i = 0; i < 9; i++)
            {
                string digit = string.Join(
                    Environment.NewLine,
                    new[]
                        {
                            line1.Substring(i * 3, 3), 
                            line2.Substring(i * 3, 3), 
                            line3.Substring(i * 3, 3)
                        });

                yield return digit;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static void AssertLength(string line)
        {
            if (string.IsNullOrEmpty(line) || line.Length != 27)
            {
                throw new FormatException("Each line containing OCR data must be exactly 27 characters long and contain only pipes ('|'), underscores ('_') or blanks (' ').");
            }
        }
    }
}