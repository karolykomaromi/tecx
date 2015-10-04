namespace Hydra.Cooling
{
    using System;
    using System.Diagnostics.Contracts;

    public class DeviceId : IEquatable<DeviceId>
    {
        public static readonly DeviceId Empty = new DeviceId();

        private readonly string id;

        public DeviceId(string id)
        {
            Contract.Requires(!string.IsNullOrEmpty(id));

            this.id = id;
        }

        private DeviceId()
        {
            this.id = string.Empty;
        }

        public bool Equals(DeviceId other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.id, other.id, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            DeviceId other = obj as DeviceId;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override string ToString()
        {
            return this.id;
        }
    }
}