using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.Data.Tfs
{
    public class TfsRepository : IRepository
    {
        private readonly Uri _tfsUri;

        public TfsRepository(Uri tfsUri)
        {
            Guard.AssertNotNull(tfsUri, "tfsUri");

            // Connect to Team Foundation Server
            //     Server is the name of the server that is running the application tier for Team Foundation.
            //     Port is the port that Team Foundation uses. The default port is 8080.
            //     VDir is the virtual path to the Team Foundation application. The default path is tfs.
            _tfsUri = tfsUri;
        }

        #region Implementation of IRepository

        public Project GetProjectBy(Guid id)
        {
            throw new NotImplementedException();
        }

        public Project GetProjectBy(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProjectInfo> GetExistingProjects()
        {
            List<ProjectInfo> projects = new List<ProjectInfo>();

            try
            {
                TfsConfigurationServer configurationServer =
                    TfsConfigurationServerFactory.GetConfigurationServer(_tfsUri);

                // Get the catalog of team project collections
                ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.ProjectCollection },
                    false, CatalogQueryOptions.None);

                // List the team project collections
                foreach (CatalogNode collectionNode in collectionNodes)
                {
                    // Use the InstanceId property to get the team project collection
                    Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                    TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                    // Print the name of the team project collection
                    Console.WriteLine(@"Collection: " + teamProjectCollection.Name);

                    // Get a catalog of team projects for the collection
                    ReadOnlyCollection<CatalogNode> projectNodes = collectionNode.QueryChildren(
                        new[] { CatalogResourceTypes.TeamProject },
                        false, CatalogQueryOptions.None);

                    // List the team projects in the collection
                    foreach (CatalogNode projectNode in projectNodes)
                    {
                        //Console.WriteLine(@" Team Project: " + projectNode.Resource.DisplayName);

                        ProjectInfo info = new ProjectInfo { Name = projectNode.Resource.DisplayName };

                        projects.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.WithAdditionalInfo("tfsUri", _tfsUri);
                throw;
            }

            return projects;
        }

        public Project CreateProject()
        {
            throw new NotImplementedException();
        }

        public bool SaveProject(Project project)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
