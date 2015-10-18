namespace Hydra.Cooling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ComPort : IEquatable<ComPort>
    {
        public static readonly ComPort Com1 = new ComPort();

        public static readonly ComPort Com2 = new ComPort();

        public static readonly ComPort Com3 = new ComPort();

        public static readonly ComPort Com4 = new ComPort();

        public static readonly ComPort Com5 = new ComPort();

        public static readonly ComPort Com6 = new ComPort();

        public static readonly ComPort Com7 = new ComPort();

        public static readonly ComPort Com8 = new ComPort();

        public static readonly ComPort Com9 = new ComPort();

        private static readonly Lazy<IReadOnlyCollection<ComPort>> WellKnownPortsLazy = new Lazy<IReadOnlyCollection<ComPort>>(ComPort.GetWellKnownPorts);

        private readonly string name;

        private ComPort([CallerMemberName] string name = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            this.name = name.ToUpperInvariant();
        }

        public static IReadOnlyCollection<ComPort> WellKnownPorts
        {
            get { return ComPort.WellKnownPortsLazy.Value; }
        }

        public static implicit operator string(ComPort comPort)
        {
            Contract.Requires(comPort != null);

            return comPort.ToString();
        }

        public static bool operator ==(ComPort x, ComPort y)
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

        public static bool operator !=(ComPort x, ComPort y)
        {
            return !(x == y);
        }

        public bool Equals(ComPort other)
        {
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.name, other.name, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            ComPort other = obj as ComPort;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        public override string ToString()
        {
            return this.name;
        }

        private static IReadOnlyCollection<ComPort> GetWellKnownPorts()
        {
            ComPort[] wellKnownPorts = typeof(ComPort)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(ComPort))
                .Select(f => f.GetValue(null))
                .OfType<ComPort>()
                .ToArray();

            return wellKnownPorts;
        }
    }
}