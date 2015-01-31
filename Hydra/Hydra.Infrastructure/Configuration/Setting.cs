namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class Setting : IEquatable<Setting>, IComparable<Setting>
    {
        public static readonly Setting Empty = new Setting(SettingName.Empty, Missing.Value);

        private readonly SettingName name;
        private readonly object value;

        public Setting(SettingName name, object value)
        {
            Contract.Requires(name != null);
            Contract.Requires(value != null);

            this.name = name;
            this.value = value;
        }

        public SettingName Name
        {
            get
            {
                Contract.Ensures(Contract.Result<SettingName>() != null);
                return this.name;
            }
        }

        public object Value
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);
                return this.value;
            }
        }

        public int CompareTo(Setting other)
        {
            Contract.Requires(other != null);

            return this.Name.CompareTo(other.Name);
        }

        public bool Equals(Setting other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (!this.Name.Equals(other.Name))
            {
                return false;
            }

            return EqualityComparer.Equals(this.Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            Setting other = obj as Setting;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = this.Name.GetHashCode() ^ this.Value.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("{0}='{1}'", this.Name, this.Value);
        }
    }
}
