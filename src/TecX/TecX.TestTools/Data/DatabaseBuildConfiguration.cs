namespace TecX.TestTools.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using TecX.Common;

    public class DatabaseBuildConfiguration
    {
        private readonly List<Action> actions;

        private bool dontDropDatabaseOnDispose;

        public DatabaseBuildConfiguration()
        {
            this.actions = new List<Action>();
        }

        private ICollection<Action> Actions
        {
            get
            {
                return this.actions;
            }
        }

        private string ConnectionString { get; set; }

        private string Database { get; set; }

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

        private Action GetDropDatabaseAction()
        {
            Action action = () =>
            {
                Console.WriteLine("Droping database '{0}'.", this.Database);
            };

            return action;
        }

        public class DatabaseBuildSequenceConfiguration
        {
            private readonly DatabaseBuildConfiguration config;

            public DatabaseBuildSequenceConfiguration(DatabaseBuildConfiguration config)
            {
                Guard.AssertNotNull(config, "config");

                this.config = config;
            }

            public void DropExistingDatabase()
            {
                Action action = this.config.GetDropDatabaseAction();

                this.config.Actions.Add(action);
            }

            public void CreateEmptyDatabase()
            {
                Action action = () =>
                {
                    Console.WriteLine("Creating empty database '{0}'.", this.config.Database);
                };

                this.config.Actions.Add(action);
            }

            public void CreateTables()
            {
                Action action = () =>
                {
                    Console.WriteLine("Creating tables.");
                };

                this.config.Actions.Add(action);
            }

            public void CreateTestData()
            {
                Action action = () =>
                {
                    Console.WriteLine("Creating test data.");
                };

                this.config.Actions.Add(action);
            }
        }
    }
}