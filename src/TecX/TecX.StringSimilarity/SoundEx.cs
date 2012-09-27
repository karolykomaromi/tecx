namespace TecX.StringSimilarity
{
    using System.Text;

    public class SoundEx : IStringSimilarityAlgorithm
    {
        public double GetSimilarity(string s, string t)
        {
            int matches = 0;
            string soundex1 = this.GetSoundEx(s);
            string soundex2 = this.GetSoundEx(t);

            for (int i = 0; i < 4; i++)
            {
                if (soundex1[i] == soundex2[i])
                {
                    matches++;
                }
            }

            return (double)matches / 4;
        }

        public string GetSoundEx(string s)
        {
            s = s ?? string.Empty;

            s = s.ToUpper().PadRight(4, '0');

            StringBuilder sb = new StringBuilder(s.Substring(1));

            // remove whitespaces and punctuation
            sb.Replace(".", string.Empty);
            sb.Replace(" ", string.Empty);

            sb.Replace('A', '0');
            sb.Replace('E', '0');
            sb.Replace('I', '0');
            sb.Replace('O', '0');
            sb.Replace('U', '0');
            sb.Replace('H', '0');
            sb.Replace('W', '0');
            sb.Replace('Y', '0');

            // german extensions
            sb.Replace('Ä', '0');
            sb.Replace('Ö', '0');
            sb.Replace('Ü', '0');
            sb.Replace('ß', '2');

            sb.Replace('B', '1');
            sb.Replace('F', '1');
            sb.Replace('P', '1');
            sb.Replace('V', '1');

            sb.Replace('C', '2');
            sb.Replace('G', '2');
            sb.Replace('J', '2');
            sb.Replace('K', '2');
            sb.Replace('Q', '2');
            sb.Replace('S', '2');
            sb.Replace('X', '2');
            sb.Replace('Z', '2');

            sb.Replace('D', '3');
            sb.Replace('T', '3');

            sb.Replace('L', '4');

            sb.Replace('M', '5');
            sb.Replace('N', '5');

            sb.Replace('R', '6');


            sb.Replace("00", "0");
            sb.Replace("11", "1");
            sb.Replace("22", "2");
            sb.Replace("33", "3");
            sb.Replace("44", "4");
            sb.Replace("55", "5");
            sb.Replace("66", "6");

            sb.Replace("0", string.Empty);

            sb.Insert(0, s.Substring(0, 1));

            string soundex = sb.ToString().PadRight(4, '0').Substring(0, 4);

            return soundex;
        }
    }
}