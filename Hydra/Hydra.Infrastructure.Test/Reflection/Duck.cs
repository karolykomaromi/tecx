namespace Hydra.Infrastructure.Test.Reflection
{
    using System;

    public class Duck : IDuck
    {
        public int NotImplementedProperty
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Foo
        {
            get; set;
        }

        public string Bar
        {
            get; set;
        }

        public void Baz(object sender, EventArgs args)
        {
        }

        public int TheAnswer()
        {
            return 42;
        }

        public void NotImplementedMethod()
        {
            throw new NotImplementedException();
        }
    }
}