namespace Hydra.Infrastructure.Test.Reflection
{
    using System;

    public class DuckButNot
    {
        public bool CalledBuz { get; private set; }

        public int Foo { get; set; }

        public string Bar { get; set; }

        public void Baz(object sender, EventArgs args)
        {
            this.CalledBuz = true;
        }

        public int TheAnswer()
        {
            return 42;
        }
    }
}