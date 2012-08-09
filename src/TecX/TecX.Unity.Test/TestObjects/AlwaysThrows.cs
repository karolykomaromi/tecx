namespace TecX.Unity.Test.TestObjects
{
    using System;

    public class AlwaysThrows : IAlwaysThrows
    {
        public AlwaysThrows()
        {
            throw new Exception("Bang!");
        }
    }
}