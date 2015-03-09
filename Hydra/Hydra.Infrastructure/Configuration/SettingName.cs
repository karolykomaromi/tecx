namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    [TypeConverter(typeof(SettingNameTypeConverter))]
    public class SettingName : IEquatable<SettingName>, IComparable<SettingName>, ICloneable<SettingName>
    {
        public static readonly SettingName Empty = new EmptySettingName();

        private readonly string group;

        private readonly string name;

        private readonly string fullName;

        private SettingName(string group, string name, string fullName)
        {
            this.group = group;
            this.name = name;
            this.fullName = fullName;
        }

        public string Group
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return this.group;
            }
        }

        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return this.name;
            }
        }

        public string FullName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return this.fullName;
            }
        }

        public static bool TryParse(string s, out SettingName settingName)
        {
            Contract.Requires(s != null);

            string[] parts = s.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
            {
                settingName = SettingName.Empty;
                return false;
            }

            string group = string.Join(".", parts.Take(parts.Length - 1).Select(p => p.ToUpperInvariant()));
            string name = parts.Last().ToUpperInvariant();

            settingName = new SettingName(group, name, group + "." + name);
            return true;
        }

        public SettingName Clone()
        {
            SettingName clone = new SettingName(this.group, this.name, this.fullName);

            return clone;
        }

        public int CompareTo(SettingName other)
        {
            if (other == null)
            {
                return 1;
            }

            return string.Compare(this.FullName, other.FullName, StringComparison.Ordinal);
        }

        public bool Equals(SettingName other)
        {
            if (other == null)
            {
                return false;
            }

            bool equals = string.Equals(this.fullName, other.fullName, StringComparison.Ordinal);

            return equals;
        }

        public override bool Equals(object obj)
        {
            SettingName other = obj as SettingName;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.fullName.GetHashCode();
        }

        public override string ToString()
        {
            return this.fullName;
        }

        private class EmptySettingName : SettingName
        {
            public EmptySettingName()
                : base(string.Empty, string.Empty, string.Empty)
            {
            }
        }
    }
}