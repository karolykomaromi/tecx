namespace TecX.TestTools.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using TecX.Common;

    public class DatabaseBuildConfiguration
    {
        private readonly IAppendOnlyCollection<Action> actions;

        private bool dontDropDatabaseOnDispose;

        public DatabaseBuildConfiguration()
        {
            this.actions = new AppendOnlyCollection<Action>();
        }

        public IAppendOnlyCollection<Action> Actions
        {
            get
            {
                return this.actions;
            }
        }

        public string ConnectionString { get; private set; }

        public string Database { get; private set; }

        public void DontDropDatabaseOnDispose()
        {
            this.dontDropDatabaseOnDispose = true;
        }

        public DatabaseBuildResult Execute()
        {
            Action onDispose = this.dontDropDatabaseOnDispose ? () => { } : this.GetDropDatabaseAction();

            var result = new DatabaseBuildResult(onDispose);

            try
            {
                foreach (Action action in this.Actions)
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }

        public void WithConnectionStringNamed(string connectionStringName)
        {
            Guard.AssertNotEmpty(connectionStringName, "connectionStringName");

            Action action = () =>
                {
                    Console.WriteLine("Retrieving ConnectionString named '{0}'.", connectionStringName);

                    this.ConnectionString =
                        ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                };

            this.Actions.Add(action);
        }

        public void WithDatabaseName(string databaseName)
        {
            Guard.AssertNotEmpty(databaseName, "databaseName");

            Action action = () =>
                {
                    Console.WriteLine("Using database name '{0}'.", databaseName);

                    this.Database = databaseName;
                };

            this.Actions.Add(action);
        }

        public void BuildSequence(Action<DatabaseBuildSequenceConfiguration> action)
        {
            var config = new DatabaseBuildSequenceConfiguration(this);

            action(config);
        }

        public Action GetDropDatabaseAction()
        {
            Action action = () =>
            {
                Console.WriteLine("Droping database '{0}'.", this.Database);
            };

            return action;
        }
    }
}