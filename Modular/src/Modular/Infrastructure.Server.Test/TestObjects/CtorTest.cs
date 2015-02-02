using System;

namespace Infrastructure.Server.Test.TestObjects
{
    internal class CtorTest : ICtorTest
    {
        public CtorTest(ILogger logger)
        {
            throw new InvalidOperationException("Must not be called");
        }

        public CtorTest(string connectionString, ILogger logger)
        {
            this.ConnectionString = connectionString;
            this.Logger = logger;
        }

        public CtorTest(string connectionString, ILogger logger, int anotherParam)
        {
            throw new InvalidOperationException("Must not be called");
        }

        public CtorTest(string connectionString, ILogger logger, string anotherParam)
        {
            this.ConnectionString = connectionString;
            this.Logger = logger;
            this.AnotherParam = anotherParam;
        }

        public string ConnectionString { get; private set; }

        public ILogger Logger { get; private set; }

        public string AnotherParam { get; private set; }
    }
}