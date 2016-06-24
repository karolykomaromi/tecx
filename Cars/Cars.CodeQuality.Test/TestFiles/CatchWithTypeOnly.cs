namespace Cars.CodeQuality.Test.TestFiles
{
    using System;

    class CatchWithTypeOnly
    {
        public void Foo()
        {
            try
            {
            }
            catch (NotFiniteNumberException)
            {
            }
        }
    }
}
