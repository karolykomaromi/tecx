namespace TecX.EnumClasses.Test.TestObjects
{
    public class Numbers : Flags<Numbers>
    {
        public static readonly Numbers None = new Numbers(0, "None");

        public static readonly Numbers One = new Numbers(1, "One");

        public static readonly Numbers Two = new Numbers(2, "Two");

        public static readonly Numbers Four = new Numbers(4, "Four");

        private Numbers(int value, string name)
            : base(value, name, name)
        {
        }

        protected override Numbers ToEnumeration()
        {
            return this;
        }

        protected override Numbers Or(Numbers x)
        {
            return new OredNumbers(this, x);
        }

        private class OredNumbers : Numbers
        {
            private readonly Or<Numbers> flags;

            public OredNumbers(Numbers x, Numbers y)
                : base(-1, string.Empty)
            {
                this.flags = new Or<Numbers>(x, y);
            }

            public override string Name
            {
                get { return this.flags.Name; }
            }

            public override string DisplayName
            {
                get { return this.flags.DisplayName; }
            }

            public override int Value
            {
                get { return this.flags.Value; }
            }

            protected override Numbers[] ToArray()
            {
                return this.flags.ToArray();
            }

            protected override Numbers Or(Numbers x)
            {
                this.flags.Add(x);

                return this;
            }
        }
    }
}