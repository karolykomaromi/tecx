namespace Hydra.Infrastructure.Test
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [TypeConverter(typeof(FlagsConverter<Colors>))]
    public class Colors : Flags<Colors>
    {
        public static readonly Colors None = new Colors();

        public static readonly Colors Red = new Colors();

        public static readonly Colors Green = new Colors();

        public static readonly Colors Blue = new Colors();

        private Colors([CallerMemberName] string name = "", [CallerLineNumber] int sortKey = -1)
            : base(name, sortKey)
        {
        }

        public static Colors operator |(Colors x, Colors y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Colors>() != null);

            return new CompositeColors(x, y);
        }

        public static Colors operator &(Colors x, Colors y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Colors>() != null);

            if (object.ReferenceEquals(x, y))
            {
                return x;
            }

            CompositeColors cx = x as CompositeColors;

            CompositeColors cy = y as CompositeColors;

            if (cx != null && cy != null)
            {
                return cx & cy;
            }

            if (cx != null)
            {
                return cx & y;
            }

            if (cy != null)
            {
                return x & cy;
            }

            return Enumeration<Colors>.Default;
        }

        public override Colors Or(Colors other)
        {
            if (other == null)
            {
                return this;
            }

            return new CompositeColors(this, other);
        }

        private class CompositeColors : Colors
        {
            private readonly HashSet<Colors> elements;

            public CompositeColors(Colors x, Colors y)
                : base("COMPOSITE", Enumeration<Colors>.Composite)
            {
                Contract.Requires(x != null);
                Contract.Requires(y != null);

                this.elements = new HashSet<Colors>();

                this.Add(x);
                this.Add(y);
            }

            public override string Name
            {
                get
                {
                    Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                    var flags = this.elements.OrderBy(e => e.Value).ToArray();

                    if (flags.Length == 1)
                    {
                        return flags[0].Name;
                    }

                    return string.Join(", ", flags.Where(f => f.Value > 0).Select(e => e.Name));
                }
            }

            public override int Value
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return this.elements.Sum(e => e.Value);
                }
            }

            public static Colors operator &(CompositeColors x, Colors y)
            {
                Contract.Requires(x != null);
                Contract.Requires(x != null);
                Contract.Ensures(Contract.Result<Colors>() != null);

                if (x.elements.Contains(y))
                {
                    return y;
                }

                return Enumeration<Colors>.Default;
            }

            public static Colors operator &(Colors x, CompositeColors y)
            {
                Contract.Requires(x != null);
                Contract.Requires(x != null);
                Contract.Ensures(Contract.Result<Colors>() != null);

                if (y.elements.Contains(x))
                {
                    return x;
                }

                return Enumeration<Colors>.Default;
            }

            public static Colors operator &(CompositeColors x, CompositeColors y)
            {
                Contract.Requires(x != null);
                Contract.Requires(x != null);
                Contract.Ensures(Contract.Result<Colors>() != null);

                var intersection = x.elements.Intersect(y.elements).ToArray();

                if (intersection.Length == 1)
                {
                    Colors i = intersection[0];

                    return i;
                }

                if (intersection.Length == 2)
                {
                    CompositeColors composite = new CompositeColors(intersection[0], intersection[1]);

                    return composite;
                }

                if (intersection.Length > 2)
                {
                    CompositeColors composite = new CompositeColors(intersection[0], intersection[1]);

                    foreach (Colors i in intersection.Skip(2))
                    {
                        composite.elements.Add(i);
                    }

                    return composite;
                }

                return Enumeration<Colors>.Default;
            }

            private void Add(Colors element)
            {
                var other = element as CompositeColors;

                if (other != null)
                {
                    foreach (Colors e in other.elements)
                    {
                        this.elements.Add(e);
                    }
                }
                else
                {
                    this.elements.Add(element);
                }
            }
        }
    }
}