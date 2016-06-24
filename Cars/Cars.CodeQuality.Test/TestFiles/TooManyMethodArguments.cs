namespace Cars.CodeQuality.Test.TestFiles
{
    using System;

    class TooManyMethodArguments
    {
        public void Foo(int one, string two, byte[] three, long four, object five, Type six)
        {
        }
    }
}
