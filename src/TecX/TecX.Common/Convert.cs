namespace TecX.Common
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Performs conversions between differnt types
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Converts an array of bytes to a hexadecimal string
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <returns>A hexadecimal string</returns>
        public static string ToHex(byte[] bytes)
        {
            // input validation
            Guard.AssertNotNull(bytes, "bytes");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                // X2 defines hex format
                string str = bytes[i].ToString("X2");
                if (!string.IsNullOrEmpty(str))
                {
                    // no separator as first letter
                    if (i > 0)
                    {
                        sb.Append("-");
                    }

                    sb.Append(str);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a hexadecimal string to an array of bytes
        /// </summary>
        /// <param name="hexString">The hexadecimal string</param>
        /// <returns>An array of bytes</returns>
        public static byte[] ToByte(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                return new byte[0];
            }

            // hex strings may start with '#' as delimiter
            hexString = hexString.TrimStart('#');

            List<byte> bytes = new List<byte>();

            if (hexString.Contains("-"))
            {
                string[] byteStrings = hexString.Split('-');

                foreach (string str in byteStrings)
                {
                    if (!string.IsNullOrEmpty(str) && (str != "-"))
                    {
                        byte b;
                        if (byte.TryParse(str, NumberStyles.HexNumber, null, out b))
                        {
                            bytes.Add(b);
                        }
                    }
                }

                return bytes.ToArray();
            }

            // you need two hex digits for a byte so the
            // string length must be even
            if (hexString.Length % 2 != 0)
            {
                return new byte[0];
            }

            try
            {
                // cut the hexstring in snippets of length 2
                for (int i = 0; i < hexString.Length; i += 2)
                {
                    string str = hexString.Substring(i, 2);

                    // check if there is anything in it
                    if (!string.IsNullOrEmpty(str))
                    {
                        byte b;
                        if (byte.TryParse(str, NumberStyles.HexNumber, null, out b))
                        {
                            bytes.Add(b);
                        }
                    }
                }
            }
            catch
            {
                /* intentionally left blank */
            }

            return bytes.ToArray();
        }
    }
}