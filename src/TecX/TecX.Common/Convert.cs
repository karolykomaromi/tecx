using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TecX.Common
{
    /// <summary>
    /// Performs conversions between differnt types
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Performs a safe conversion to <see cref="decimal"/>
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted value; <i>default(decimal)</i> if an error occurs</returns>
        public static decimal ToDecimal(object value)
        {
            decimal converted = ToDecimal(value, default(decimal));

            return converted;
        }

        /// <summary>
        /// Performs a safe conversion to <see cref="decimal"/>
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="def">The default value</param>
        /// <returns>The converted value; <paramref name="def"/> if an error occurs</returns>
        public static decimal ToDecimal(object value, decimal def)
        {
            if (value == null)
                return def;

            decimal result;

            //if the value already is a string -> try to parse it
            if (TypeHelper.TryParse(value as string, out result))
            {
                return result;
            }

            try
            {
                result = ToDecimal(value);
                return result;
            }
            catch
            {
                /* Intentionally left blank */
            }

            //value cannot be converted and is not a string
            if (TypeHelper.TryParse(value.ToString(), out result))
            {
                return result;
            }

            return def;
        }

        /// <summary>
        /// Tries to convert a string representation of an enum value
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="value">The string representation</param>
        /// <returns>The enum value represented by the string</returns>
        public static T ToEnum<T>(string value)
        {
            Guard.AssertNotEmpty(value, "value");

            Type enumType = typeof (T);
            T enumValue = default(T);

            try
            {
                enumValue = (T) Enum.Parse(enumType, value, true);
            }
            catch
            {
                /* intentionally left blank */
            }

            return enumValue;
        }

        /// <summary>
        /// Converts an array of bytes to a hexadecimal string
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <returns>A hexadecimal string</returns>
        public static string ToHex(byte[] bytes)
        {
            //input validation
            Guard.AssertNotNull(bytes, "bytes");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                //X2 defines hex format
                string str = bytes[i].ToString("X2");
                if (!String.IsNullOrEmpty(str))
                {
                    //no separator as first letter
                    if (i > 0)
                        sb.Append("-");

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
            if (String.IsNullOrEmpty(hexString))
            {
                return new byte[0];
            }

            //hex strings may start with '#' as delimiter
            hexString = hexString.TrimStart('#');

            List<byte> bytes = new List<byte>();

            if (hexString.Contains("-"))
            {
                string[] byteStrings = hexString.Split('-');

                for (int i = 0; i < byteStrings.Length; i++)
                {
                    string str = byteStrings[i];

                    if (!String.IsNullOrEmpty(str) && (str != "-"))
                    {
                        byte b;
                        if (Byte.TryParse(str, NumberStyles.HexNumber, null, out b))
                        {
                            bytes.Add(b);
                        }
                    }
                }

                return bytes.ToArray();
            }
            //you need two hex digits for a byte so the
            //string length must be even
            if (hexString.Length%2 != 0)
            {
                return new byte[0];
            }

            try
            {
                //cut the hexstring in snippets of length 2
                for (int i = 0; i < hexString.Length; i += 2)
                {
                    string str = hexString.Substring(i, 2);

                    //check if there is anything in it
                    if (!String.IsNullOrEmpty(str))
                    {
                        byte b;
                        if (Byte.TryParse(str, NumberStyles.HexNumber, null, out b))
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