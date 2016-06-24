namespace Cars
{
    public static class FormatStrings
    {
        public static class DateAndTime
        {
            /// <summary>
            /// <para>
            /// Short date pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#ShortDate">The Short Date ("d") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string ShortDate = "d";

            /// <summary>
            /// <para>
            /// Long date pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#LongDate">The Long Date ("D") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string LongDate = "D";

            /// <summary>
            /// <para>
            /// Full date/time pattern (short time).
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#FullDateShortTime">The Full Date Short Time ("f") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string FullDateTimeWithShortTime = "f";

            /// <summary>
            /// <para>
            /// Full date/time pattern (long time).
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#FullDateLongTime">The Full Date Long Time ("F") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string FullDateTimeWithLongTime = "F";

            /// <summary>
            /// <para>
            /// General date/time pattern (short time).
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#GeneralDateShortTime">The General Date Short Time ("g") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string GeneralDateTimeWithShortTime = "g";

            /// <summary>
            /// <para>
            /// General date/time pattern (long time).
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#GeneralDateLongTime">The General Date Long Time ("G") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string GeneralDateTimeWithLongTime = "G";

            /// <summary>
            /// <para>
            /// Month/day pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#MonthDay">The Month ("M", "m") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string MonthDay = "M";

            /// <summary>
            /// <para>
            /// Round-trip date/time pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Roundtrip">The Round-trip ("O", "o") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string RoundTrip = "O";

            /// <summary>
            /// <para>
            /// RFC1123 pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#RFC1123">The RFC1123 ("R", "r") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string RFC1123 = "R";

            /// <summary>
            /// <para>
            /// Sortable date/time pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Sortable">The Sortable ("s") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string SortableDateTime = "s";

            /// <summary>
            /// <para>
            /// Short time pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#ShortTime">The Short Time ("t") Format Specifier. </see>
            /// </para>
            /// </summary>
            public const string ShortTime = "t";

            /// <summary>
            /// <para>
            /// Long time pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#LongTime">The Long Time ("T") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string LongTime = "T";

            /// <summary>
            /// <para>
            /// Universal sortable date/time pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#UniversalSortable">The Universal Sortable ("u") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string UniversalSortableDateTime = "u";

            /// <summary>
            /// <para>
            /// Universal full date/time pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#UniversalFull">The Universal Full ("U") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string UniversalFullDateTime = "U";

            /// <summary>
            /// <para>
            /// Year month pattern.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#YearMonth">The Year Month ("Y") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string YearMonth = "M";
        }

        public static class TimeSpan
        {
            /// <summary>
            /// <para>
            /// Constant (invariant) format
            /// </para>
            /// <para>
            ///  More information: <see href="https://msdn.microsoft.com/en-us/library/ee372286(v=vs.110).aspx#Constant">The Constant ("c") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string ConstantInvariant = "c";

            /// <summary>
            /// <para>
            /// General short format
            /// </para>
            /// <para>
            ///  More information: <see href="https://msdn.microsoft.com/en-us/library/ee372286(v=vs.110).aspx#GeneralShort">The General Short ("g") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string GeneralShort = "g";

            /// <summary>
            /// <para>
            /// General long format
            /// </para>
            /// <para>
            ///  More information: <see href="https://msdn.microsoft.com/en-us/library/ee372286(v=vs.110).aspx#GeneralLong">The General Long ("G") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string GeneralLong = "G";
        }

        public static class Guid
        {
            /// <summary>
            /// <para>
            /// 32 digits: 00000000000000000000000000000000 
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/97af8hh4.aspx">The Digits ("N") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Digits = "N";

            /// <summary>
            /// <para>
            /// 32 digits separated by hyphens: 00000000-0000-0000-0000-000000000000 
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/97af8hh4.aspx">The Digits and Hyphens ("D") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Hyphens = "D";

            /// <summary>
            /// <para>
            /// 32 digits separated by hyphens, enclosed in braces: {00000000-0000-0000-0000-000000000000} 
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/97af8hh4.aspx">The Hyphens and Braces ("B") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string HyphensAndBraces = "B";

            /// <summary>
            /// <para>
            /// 32 digits separated by hyphens, enclosed in parentheses: (00000000-0000-0000-0000-000000000000) 
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/97af8hh4.aspx">The Hyphens and Parentheses ("P") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string HyphensAndParentheses = "P";

            /// <summary>
            /// <para>
            /// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces: 
            /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/97af8hh4.aspx">The Hex ("X") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Hex = "X";
        }

        public static class Numeric
        {
            /// <summary>
            /// <para>
            /// Result: A currency value.
            /// </para>
            /// <para>
            /// Supported by: All numeric types.
            /// </para>
            /// <para>
            /// Precision specifier: Number of decimal digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="System.Globalization.NumberFormatInfo">System.Globalization.NumberFormatInfo</see>.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#CFormatString">The Currency ("C") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Currency = "C";

            /// <summary>
            /// <para>
            /// Result: Integer digits with optional negative sign.
            /// </para>
            /// <para>
            /// Supported by: Integral types only.
            /// </para>
            /// <para>
            /// Precision specifier: Minimum number of digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Minimum number of digits required.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#DFormatString">The Decimal("D") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Decimal = "D";

            /// <summary>
            /// <para>
            /// Result: Exponential notation.
            /// </para>
            /// <para>
            /// Supported by: All numeric types.
            /// </para>
            /// <para>
            /// Precision specifier: Number of decimal digits.
            /// </para>
            /// <para>
            /// Default precision specifier: 6.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#EFormatString">The Exponential ("E") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Exponential = "E";

            /// <summary>
            /// <para>
            /// Result: Integral and decimal digits with optional negative sign.
            /// </para>
            /// <para>
            /// Supported by: All numeric types.
            /// </para>
            /// <para>
            /// Precision specifier: Number of decimal digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="System.Globalization.NumberFormatInfo">System.Globalization.NumberFormatInfo.</see>
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#FFormatString">The Fixed-Point ("F") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string FixedPoint = "F";

            /// <summary>
            /// <para>
            /// Result: The most compact of either fixed-point or scientific notation.
            /// </para>
            /// <para>
            /// Supported by: All numeric types.
            /// </para>
            /// <para>
            /// Precision specifier: Number of significant digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Depends on numeric type.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#GFormatString">The General ("G") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string General = "G";

            /// <summary>
            /// <para>
            /// Result: Integral and decimal digits, group separators, and a decimal separator with optional negative sign.
            /// </para>
            /// <para>
            /// Supported by: All numeric types.
            /// </para>
            /// <para>
            /// Precision specifier: Desired number of decimal places.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="System.Globalization.NumberFormatInfo">System.Globalization.NumberFormatInfo.</see>
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#NFormatString">The Numeric ("N") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Number = "N";

            /// <summary>
            /// <para>
            /// Result: Number multiplied by 100 and displayed with a percent symbol.
            /// </para>
            /// <para>
            /// Supported by: All numeric types.
            /// </para>
            /// <para>
            /// Precision specifier: Desired number of decimal places.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="System.Globalization.NumberFormatInfo">System.Globalization.NumberFormatInfo.</see>
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#PFormatString">The Percent ("P") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Percent = "P";

            /// <summary>
            /// <para>
            /// Result: A hexadecimal string.
            /// </para>
            /// <para>
            /// Supported by: Integral types only.
            /// </para>
            /// <para>
            /// Precision specifier: Number of digits in the result string.
            /// </para>
            /// <para>More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#XFormatString">The HexaDecimal ("X") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string Hex = "X";

            /// <summary>
            /// <para>
            /// Result: A string that can round-trip to an identical number.
            /// </para>
            /// <para>
            /// Supported by: <see href="https://msdn.microsoft.com/en-us/library/system.single(v=vs.110).aspx">Single</see> and <see href="https://msdn.microsoft.com/en-us/library/system.double(v=vs.110).aspx">Double</see>.
            /// </para>
            /// <para>
            /// Precision specifier: Ignored.
            /// </para>
            /// <para>
            /// More information: <see href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx#RFormatString">The Round-trip ("R") Format Specifier.</see>
            /// </para>
            /// </summary>
            public const string RoundTrip = "R";
        }

        public static class CurrencyAmount
        {
            public const string FixedPoint = "F";

            public const string Currency = "C";
        }
    }
}
