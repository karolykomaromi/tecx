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

        private readonly Action<IDuck> release;

        public DuckDecoraptor(Func<IDuck> create, Action<IDuck> release)
        {
            this.create = create;
            this.release = release;
        }

        public int Foo
        {
            get
            {
                IDuck instance = null;
                try
                {
                    instance = this.Create;
                        
                    return instance.Foo;
                }
                finally
                {
                    this.Release(instance);
                }
            }

            set
            {
                IDuck instance = null;
                try
                {
                    instance = this.Create;

                    instance.Foo = value;
                }
                finally
                {
                    this.Release(instance);
                }
            }
        }

        public IDuck Create
        {
            get { return this.create(); }
        }

        public int TheAnswer()
        {
            IDuck instance = null;
            try
            {
                instance = this.Create;
                return instance.TheAnswer();
            }
            finally
            {
                this.Release(instance);
            }
        }

        private void Release(IDuck instance)
        {
            if (instance != null)
            {
                this.release(instance);
            }
        }
    }
}