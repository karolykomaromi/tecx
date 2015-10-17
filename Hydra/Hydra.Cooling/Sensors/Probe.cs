namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    public class Probe : IEquatable<Probe>
    {
        public static readonly Probe Room = new Probe(3);

        public static readonly Probe Defrost = new Probe(4);

        public static readonly Probe Unknown1 = new Probe(5);

        public static readonly Probe Unknown2 = new Probe(6);

        public static readonly Probe Unknown3 = new Probe(7);

        private readonly int registerIndex;

        private readonly string name;

        private Probe(int registerIndex, [CallerMemberName] string name = null)
        {
            Contract.Requires(registerIndex >= 0);
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

        public static implicit operator int(Probe probe)
        {
            Contract.Requires(probe != null);

            return probe.registerIndex;
        }

        public static bool operator ==(Probe x, Probe y)
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

        public static bool operator !=(Probe x, Probe y)
        {
            return !(x == y);
        }

        public bool Equals(Probe other)
        {
            if (other == null)
            {
                return false;
            }

            return this.registerIndex == other.registerIndex;
        }

        public override bool Equals(object obj)
        {
            Probe other = obj as Probe;

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