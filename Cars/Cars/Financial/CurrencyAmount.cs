namespace Cars.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class CurrencyAmount : IEquatable<CurrencyAmount>, IComparable<CurrencyAmount>, IFormattable
    {
        private readonly decimal amount;

        private readonly Currency currency;

        public CurrencyAmount(decimal amount, Currency currency)
        {
            Contract.Requires(currency != null);

            this.amount = amount;
            this.currency = currency;
        }

        public decimal Amount
        {
            get { return this.amount; }
        }

        public Currency Currency
        {
            get { return this.currency; }
        }

        public static CurrencyAmount operator *(CurrencyAmount ca, decimal factor)
        {
            return new CurrencyAmount(ca.Amount * factor, ca.currency);
        }

        public static CurrencyAmount operator *(decimal factor, CurrencyAmount ca)
        {
            return new CurrencyAmount(ca.Amount * factor, ca.currency);
        }

        public static CurrencyAmount operator *(CurrencyAmount ca, int factor)
        {
            return new CurrencyAmount(ca.Amount * factor, ca.currency);
        }

        public static CurrencyAmount operator *(int factor, CurrencyAmount ca)
        {
            return new CurrencyAmount(ca.Amount * factor, ca.currency);
        }

        public static CurrencyAmount operator *(CurrencyAmount ca, long factor)
        {
            return new CurrencyAmount(ca.Amount * factor, ca.currency);
        }

        public static CurrencyAmount operator *(long factor, CurrencyAmount ca)
        {
            return new CurrencyAmount(ca.Amount * factor, ca.currency);
        }

        public static CurrencyAmount operator *(CurrencyAmount ca, double factor)
        {
            return new CurrencyAmount(ca.Amount * new decimal(factor), ca.currency);
        }

        public static CurrencyAmount operator *(double factor, CurrencyAmount ca)
        {
            return new CurrencyAmount(ca.Amount * new decimal(factor), ca.currency);
        }

        public static CurrencyAmount operator /(CurrencyAmount ca, decimal divisor)
        {
            return new CurrencyAmount(ca.Amount / divisor, ca.currency);
        }

        public static CurrencyAmount operator /(CurrencyAmount ca, double divisor)
        {
            return new CurrencyAmount(ca.Amount / new decimal(divisor), ca.currency);
        }

        public static CurrencyAmount operator /(CurrencyAmount ca, int divisor)
        {
            return new CurrencyAmount(ca.Amount / divisor, ca.currency);
        }

        public static CurrencyAmount operator /(CurrencyAmount ca, long divisor)
        {
            return new CurrencyAmount(ca.Amount / divisor, ca.currency);
        }

        public static bool operator ==(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            if (object.ReferenceEquals(ca1, ca2))
            {
                return true;
            }

            if (object.ReferenceEquals(ca1, null))
            {
                return false;
            }

            if (object.ReferenceEquals(ca2, null))
            {
                return false;
            }

            if (ca1.Currency != ca2.Currency)
            {
                return false;
            }

            return ca1.Amount == ca2.Amount;
        }

        public static bool operator !=(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            return !(ca1 == ca2);
        }

        public static bool operator <(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            if (object.ReferenceEquals(ca1, ca2))
            {
                return false;
            }

            if (object.ReferenceEquals(ca1, null))
            {
                return true;
            }

            if (object.ReferenceEquals(ca2, null))
            {
                return false;
            }

            AssertCurrenciesMatch(ca1, ca2);

            return ca1.Amount < ca2.Amount;
        }

        public static bool operator >(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            if (object.ReferenceEquals(ca1, ca2))
            {
                return false;
            }

            if (object.ReferenceEquals(ca1, null))
            {
                return false;
            }

            if (object.ReferenceEquals(ca2, null))
            {
                return true;
            }

            AssertCurrenciesMatch(ca1, ca2);

            return ca1.Amount > ca2.Amount;
        }

        public static bool operator <=(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            if (object.ReferenceEquals(ca1, ca2))
            {
                return true;
            }

            if (object.ReferenceEquals(ca1, null))
            {
                return true;
            }

            if (object.ReferenceEquals(ca2, null))
            {
                return false;
            }

            AssertCurrenciesMatch(ca1, ca2);

            return ca1.Amount <= ca2.Amount;
        }

        public static bool operator >=(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            if (ca1 == ca2)
            {
                return true;
            }

            if (object.ReferenceEquals(ca1, null))
            {
                return false;
            }

            if (object.ReferenceEquals(ca2, null))
            {
                return true;
            }

            AssertCurrenciesMatch(ca1, ca2);

            return ca1.Amount >= ca2.Amount;
        }

        public static CurrencyAmount operator +(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            Contract.Requires(ca1 != null);
            Contract.Requires(ca2 != null);

            AssertCurrenciesMatch(ca1, ca2);

            return new CurrencyAmount(ca1.Amount + ca2.Amount, ca1.Currency);
        }

        public static CurrencyAmount operator -(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            Contract.Requires(ca1 != null);
            Contract.Requires(ca2 != null);

            AssertCurrenciesMatch(ca1, ca2);

            return new CurrencyAmount(ca1.Amount - ca2.Amount, ca1.Currency);
        }

        public int CompareTo(CurrencyAmount other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

            AssertCurrenciesMatch(this, other);

            return this.Amount.CompareTo(other.Amount);
        }

        public bool Equals(CurrencyAmount other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Currency == other.Currency && this.Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            CurrencyAmount other = obj as CurrencyAmount;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Currency.GetHashCode() ^ this.Amount.GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format ?? string.Empty;
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            if (format.StartsWith(FormatStrings.CurrencyAmount.FixedPoint))
            {
                return this.Amount.ToString(format, formatProvider);
            }

            if (format.StartsWith(FormatStrings.CurrencyAmount.Currency))
            {
                format = format.Replace(FormatStrings.CurrencyAmount.Currency, FormatStrings.CurrencyAmount.FixedPoint);

                return this.Amount.ToString(format, formatProvider) + " " + this.Currency.ISO4217Code;
            }

            throw new FormatException(string.Format(Properties.Resources.InvalidCurrencyFormatString, format));
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return this.ToString(string.Empty, formatProvider);
        }

        public override string ToString()
        {
            return this.ToString(string.Empty, CultureInfo.CurrentCulture);
        }

        private static void AssertCurrenciesMatch(CurrencyAmount ca1, CurrencyAmount ca2)
        {
            if (ca1 != null &&
                ca2 != null &&
                ca1.Currency != ca2.Currency)
            {
                throw new CurrencyMismatchException(ca1, ca2);
            }
        }
    }
}