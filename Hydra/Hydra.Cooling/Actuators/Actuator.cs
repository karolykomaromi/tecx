namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    public class Actuator : IEquatable<Actuator>
    {
        public static readonly Actuator TargetTemperature = new Actuator(15);

        private readonly ushort registerIndex;

        private readonly string name;

        private Actuator(ushort registerIndex, [CallerMemberName] string name = null)
        {
            Contract.Requires(registerIndex <= ModbusHelper.NumRegisters - 1);
            Contract.Requires(!string.IsNullOrEmpty(name));

            this.registerIndex = registerIndex;
            this.name = name;
        }

        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return this.name;
            }
        }

        public static implicit operator ushort(Actuator actuator)
        {
            Contract.Requires(actuator != null);

            return actuator.registerIndex;
        }

        public static bool operator ==(Actuator x, Actuator y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(Actuator x, Actuator y)
        {
            return !(x == y);
        }

        public bool Equals(Actuator other)
        {
            if (other == null)
            {
                return false;
            }

            return this.registerIndex == other.registerIndex;
        }

        public override bool Equals(object obj)
        {
            Actuator other = obj as Actuator;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.registerIndex;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}