namespace TecX.TestTools.Data
{
    using System;

    using TecX.Common;

    public static class Database
    {
        public static DatabaseBuildResult Build(Action<DatabaseBuildConfiguration> action)
        {
            Guard.AssertNotNull(action, "action");

            var config = new DatabaseBuildConfiguration();

            action(config);

            return config.Execute();
        }
    }
}
