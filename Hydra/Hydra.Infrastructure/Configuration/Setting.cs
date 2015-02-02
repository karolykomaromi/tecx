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
            get { return this.value; }
        }

        public int CompareTo(Setting other)
        {
            Contract.Requires(other != null);

            return this.Name.CompareTo(other.Name);
        }

        public bool Equals(Setting other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
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
            int hashCode = this.Name.GetHashCode() ^ (this.Value == null ? 0 : this.Value.GetHashCode());

            return hashCode;
        }

        public override string ToString()
        {
            return this.Name + "='" + (this.Value != null ? this.Value.ToString() : string.Empty) + "'";
        }
    }
}
