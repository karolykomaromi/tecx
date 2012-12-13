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

        public ICollection<Action> Actions
        {
            get
            {
                return this.actions;
            }
        }

        public string ConnectionString { get; set; }

        public string Database { get; set; }

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

        private Action GetDropDatabaseAction()
        {
            return () => { };
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
    }
}