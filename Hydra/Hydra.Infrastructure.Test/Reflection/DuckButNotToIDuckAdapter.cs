namespace Hydra.Infrastructure.Test.Reflection
{
    using System;

    public class DuckButNotToIDuckAdapter : IDuck
    {
        private readonly DuckButNot target;

        public DuckButNotToIDuckAdapter(DuckButNot target)
        {
            this.target = target;
        }

        public DuckButNot Target
        {
            get
            {
                return this.target;
            }
        }

        public int NotImplementedProperty
        {
            get { throw new NotImplementedException(); }
        }

        public int Foo
        {
            get
            {
                return this.Target.Foo;
            }

            set
            {
                this.Target.Foo = value;
            }
        }

        public string Bar
        {
            get
            {
                return this.Target.Bar;
            }

            set
            {
                this.Target.Bar = value;
            }
        }

        public void Baz(object sender, EventArgs args)
        {
            this.Target.Baz(sender, args);
        }

        public int TheAnswer()
        {
            return this.Target.TheAnswer();
        }

        public void NotImplementedMethod()
        {
            throw new NotImplementedException();
        }
    }

    public class DuckDecoraptor
    {
        private readonly Func<IDuck> create;

        public DuckDecoraptor(Func<IDuck> create)
        {
            this.create = create;
        }

        public int Foo
        {
            get
            {
                return this.Create.Foo;
            }

            set
            {
                this.Create.Foo = value;
            }
        }

        public IDuck Create
        {
            get { return this.create(); }
        }

        public int TheAnswer()
        {
            return this.Create.TheAnswer();
        }
    }
}