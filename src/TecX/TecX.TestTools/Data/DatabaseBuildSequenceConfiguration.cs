using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

using TecX.Common;

namespace TecX.TestTools.Data
{
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