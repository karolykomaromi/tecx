namespace TecX.EnumClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class Or<T> where T : Flags<T>
    {
        private readonly List<T> flags;

        public Or(T x, T y)
        {
            Guard.AssertNotNull(x, "x");
            Guard.AssertNotNull(y, "y");

            this.flags = new List<T>();

            this.flags.AddRange(x.ToArray());
            this.flags.AddRange(y.ToArray());
        }

        public string Name
        {
            get
            {
                string[] names = this.flags.OrderBy(f => f.Value).Select(f => f.Name).ToArray();
                return string.Join(" ", names);
            }
        }

        public int Value
        {
            get { return this.flags.Sum(f => f.Value); }
        }

        public string DisplayName
        {
            get
            {
                string[] names = this.flags.OrderBy(f => f.Value).Select(f => f.DisplayName).ToArray();
                return string.Join(" ", names);
            }
        }

        public T[] ToArray()
        {
            return this.flags.ToArray();
        }

        public void Add(T x)
        {
            this.flags.AddRange(x.ToArray());
        }
    }
}