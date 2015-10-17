namespace Hydra.Cooling
{
    using System;
    using System.Diagnostics.Contracts;

    public class DeviceId : IEquatable<DeviceId>
    {
        public static readonly DeviceId Empty = new DeviceId();

        private readonly byte id;

        public DeviceId(byte id)
        {
            this.id = id;
        }

        private DeviceId()
        {
            this.id = byte.MinValue;
        }

        public static implicit operator byte(DeviceId deviceId)
        {
            Contract.Requires(deviceId != null);

            return deviceId.id;
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

            return this.id == other.id;
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
            return this.id.ToString();
        }
    }
}