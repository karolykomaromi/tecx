namespace Hydra.Infrastructure.Test.Reflection
{
    using System;

    public class DuckButNotToIDuckAdapter : IDuck
    {
        private readonly DuckButNot adaptee;

        public DuckButNotToIDuckAdapter(DuckButNot adaptee)
        {
            this.adaptee = adaptee;
        }

        public int Foo
        {
            get
            {
                return this.adaptee.Foo;
            }

            set
            {
                this.adaptee.Foo = value;
            }
        }

        public string Bar
        {
            get
            {
                return this.adaptee.Bar;
            }

            set
            {
                this.adaptee.Bar = value;
            }
        }

        public void Baz(object sender, EventArgs args)
        {
            this.adaptee.Baz(sender, args);
        }

        public int TheAnswer()
        {
            return this.adaptee.TheAnswer();
        }
    }
}