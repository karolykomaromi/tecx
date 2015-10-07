namespace Hydra.Cooling.Test.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Xunit;
    using Xunit.Extensions;

    public class PhoneNumberTests
    {
        [Fact]
        public void Should_Print_Empty_Phone_Number_As_Empty_String()
        {
            string actual = PhoneNumber.Empty.ToString();

            Assert.Equal(string.Empty, actual);
        }

        [Theory]
        [InlineData(49u, 721u, 47049050u, 0u, "+49 (721) 47049050")]
        public void Should_Print_Phone_Number_Correctrly(
            uint countryCode,
            uint areaCode,
            uint dialNumber,
            uint phoneExtension,
            string expected)
        {
            string actual = new PhoneNumber(new CountryCode(countryCode), new AreaCode(areaCode), new DialNumber(dialNumber), new PhoneExtension(phoneExtension)).ToString();

            Assert.Equal(expected, actual);
        }
    }

    public class AreaCodeTests
    {
        
    }

    public class PhoneNumber : IEquatable<PhoneNumber>
    {
        public static readonly PhoneNumber Empty = new PhoneNumber();

        private readonly CountryCode countryCode;

        private readonly AreaCode areaCode;

        private readonly DialNumber dialNumber;

        private readonly PhoneExtension phoneExtension;

        public PhoneNumber(
            CountryCode countryCode,
            AreaCode areaCode,
            DialNumber dialNumber)
            : this(countryCode, areaCode, dialNumber, PhoneExtension.Empty)
        {
        }

        public PhoneNumber(
            CountryCode countryCode,
            AreaCode areaCode,
            DialNumber dialNumber,
            PhoneExtension phoneExtension)
        {
            Contract.Requires(countryCode != null);
            Contract.Requires(countryCode != CountryCode.Empty);
            Contract.Requires(areaCode != null);
            Contract.Requires(areaCode != AreaCode.Empty);
            Contract.Requires(dialNumber != null);
            Contract.Requires(dialNumber != DialNumber.Empty);
            Contract.Requires(phoneExtension != null);

            this.countryCode = countryCode;
            this.areaCode = areaCode;
            this.dialNumber = dialNumber;
            this.phoneExtension = phoneExtension;
        }

        private PhoneNumber()
        {
            this.countryCode = CountryCode.Empty;
            this.areaCode = AreaCode.Empty;
            this.dialNumber = DialNumber.Empty;
            this.phoneExtension = PhoneExtension.Empty;
        }

        public CountryCode CountryCode
        {
            get
            {
                Contract.Ensures(Contract.Result<CountryCode>() != null);

                return this.countryCode;
            }
        }

        public AreaCode AreaCode
        {
            get
            {
                Contract.Ensures(Contract.Result<AreaCode>() != null);

                return this.areaCode;
            }
        }

        public DialNumber DialNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<DialNumber>() != null);

                return this.dialNumber;
            }
        }

        public PhoneExtension PhoneExtension
        {
            get
            {
                Contract.Ensures(Contract.Result<PhoneExtension>() != null);

                return this.phoneExtension;
            }
        }

        public static bool operator ==(PhoneNumber first, PhoneNumber second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null))
            {
                return false;
            }

            if (object.ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(PhoneNumber first, PhoneNumber second)
        {
            return !(first == second);
        }

        public bool Equals(PhoneNumber other)
        {
            if (other == null)
            {
                return false;
            }

            if (!this.CountryCode.Equals(other.CountryCode))
            {
                return false;
            }

            if (!this.AreaCode.Equals(other.AreaCode))
            {
                return false;
            }

            if (!this.DialNumber.Equals(other.DialNumber))
            {
                return false;
            }

            if (!this.PhoneExtension.Equals(other.PhoneExtension))
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            PhoneNumber other = obj as PhoneNumber;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.CountryCode.GetHashCode() ^ 
                this.AreaCode.GetHashCode() ^ 
                this.DialNumber.GetHashCode() ^ 
                this.PhoneExtension.GetHashCode();
        }

        public override string ToString()
        {
            string phoneNumber = string.Join(" ", this.CountryCode.ToString(), this.AreaCode.ToString(), this.DialNumber.ToString(), this.PhoneExtension.ToString());

            return phoneNumber.Trim();
        }
    }

    public class CountryCodeTests
    {
        [Fact]
        public void Should_Print_Empty_CountryCode_As_Empty_String()
        {
            string actual = CountryCode.Empty.ToString();

            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void Should_Return_German_Country_Code()
        {
            CountryCode actual;

            Assert.True(CountryCode.TryGetCountryCode(Cultures.GermanGermany, out actual));
            Assert.Equal(CountryCodes.Germany, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.GermanNeutral, out actual));
            Assert.Equal(CountryCodes.Germany, actual);
        }
    }

    public class AreaCode : IEquatable<AreaCode>, IComparable<AreaCode>
    {
        public static readonly AreaCode Empty = new AreaCode();

        private readonly uint areaCode;

        public AreaCode(uint areaCode)
        {
            this.areaCode = areaCode;
        }

        private AreaCode()
        {
        }

        public static bool operator ==(AreaCode first, AreaCode second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null))
            {
                return false;
            }

            if (object.ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(AreaCode first, AreaCode second)
        {
            return !(first == second);
        }

        public int CompareTo(AreaCode other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.areaCode.CompareTo(other.areaCode);
        }

        public bool Equals(AreaCode other)
        {
            if (other == null)
            {
                return false;
            }

            return this.areaCode == other.areaCode;
        }

        public override bool Equals(object obj)
        {
            AreaCode other = obj as AreaCode;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.areaCode.GetHashCode();
        }

        public override string ToString()
        {
            if (this.areaCode > 0)
            {
                return "(" + this.areaCode + ")";
            }

            return string.Empty;
        }
    }

    public class DialNumber : IEquatable<DialNumber>, IComparable<DialNumber>
    {
        public static readonly DialNumber Empty = new DialNumber();

        private readonly uint dialNumber;

        public DialNumber(uint dialNumber)
        {
            this.dialNumber = dialNumber;
        }

        private DialNumber()
        {
        }

        public static bool operator ==(DialNumber first, DialNumber second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null))
            {
                return false;
            }

            if (object.ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(DialNumber first, DialNumber second)
        {
            return !(first == second);
        }

        public int CompareTo(DialNumber other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.dialNumber.CompareTo(other.dialNumber);
        }

        public bool Equals(DialNumber other)
        {
            if (other == null)
            {
                return false;
            }

            return this.dialNumber == other.dialNumber;
        }

        public override bool Equals(object obj)
        {
            DialNumber other = obj as DialNumber;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.dialNumber.GetHashCode();
        }

        public override string ToString()
        {
            if (this.dialNumber > 0)
            {
                return this.dialNumber.ToString();
            }

            return string.Empty;
        }
    }

    public class PhoneExtension : IEquatable<PhoneExtension>, IComparable<PhoneExtension>
    {
        public static readonly PhoneExtension Empty = new PhoneExtension();

        private readonly uint phoneExtension;

        public PhoneExtension(uint phoneExtension)
        {
            this.phoneExtension = phoneExtension;
        }

        private PhoneExtension()
        {
        }

        public static bool operator ==(PhoneExtension first, PhoneExtension second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null))
            {
                return false;
            }

            if (object.ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(PhoneExtension first, PhoneExtension second)
        {
            return !(first == second);
        }

        public int CompareTo(PhoneExtension other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.phoneExtension.CompareTo(other.phoneExtension);
        }

        public bool Equals(PhoneExtension other)
        {
            if (other == null)
            {
                return false;
            }

            return this.phoneExtension == other.phoneExtension;
        }

        public override bool Equals(object obj)
        {
            PhoneExtension other = obj as PhoneExtension;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.phoneExtension.GetHashCode();
        }

        public override string ToString()
        {
            if (this.phoneExtension > 0)
            {
                return "-" + this.phoneExtension;
            }

            return string.Empty;
        }
    }

    public class CountryCode : IEquatable<CountryCode>, IComparable<CountryCode>
    {
        public static readonly CountryCode Empty = new CountryCode();

        private readonly uint countryCode;

        public CountryCode(uint countryCode)
        {
            this.countryCode = countryCode;
        }

        private CountryCode()
        {
        }

        public static bool operator ==(CountryCode first, CountryCode second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null))
            {
                return false;
            }

            if (object.ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(CountryCode first, CountryCode second)
        {
            return !(first == second);
        }

        public static bool TryGetCountryCode(CultureInfo culture, out CountryCode countryCode)
        {
            if (culture == null)
            {
                countryCode = CountryCode.Empty;
                return false;
            }

            if (culture.Equals(Cultures.GermanGermany) || culture.Equals(Cultures.GermanNeutral))
            {
                countryCode = CountryCodes.Germany;
                return true;
            }

            countryCode = CountryCode.Empty;
            return false;
        }

        public int CompareTo(CountryCode other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.countryCode.CompareTo(other.countryCode);
        }

        public bool Equals(CountryCode other)
        {
            if (other == null)
            {
                return false;
            }

            return this.countryCode == other.countryCode;
        }

        public override bool Equals(object obj)
        {
            CountryCode other = obj as CountryCode;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.countryCode.GetHashCode();
        }

        public override string ToString()
        {
            if (this.countryCode > 0)
            {
                return "+" + this.countryCode;
            }

            return string.Empty;
        }
    }

    public class CountryCodes
    {
        public static readonly CountryCode Germany = new CountryCode(49);

        public static readonly CountryCode Austria = new CountryCode(43);

        public static readonly CountryCode Switzerland = new CountryCode(41);

        public static readonly CountryCode France = new CountryCode(33);

        public static readonly CountryCode UnitedKingdom = new CountryCode(44);
    }
}
