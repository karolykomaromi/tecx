namespace TecX.Unity.Test.TestObjects
{
    using System;

    public class Foo2
    {
        public IBar Bar { get; set; }

        public IBar Bar2 { get; private set; }

        public string this[int index]
        {
            get
            {
                return string.Empty;
            }

            set
            {
                throw new InvalidOperationException("must not be injected");
            }
        }
    }
}