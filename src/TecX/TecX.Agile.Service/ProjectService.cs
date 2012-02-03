namespace TecX.Agile.Service
{
    using System;

    using TecX.Agile.Service.Results;

    public class ProjectService : IProjectService
    {
        public ProjectQueryResult GetProjects(int startingFromIndex, int takeCount)
        {
            throw new System.NotImplementedException();
        }

        public IterationQueryResult GetIterations(int startingFromIndex, int takeCount, Guid projectId)
        {
            throw new System.NotImplementedException();
        }

        public StoryQueryResult GetUserStories(int startingFromIndex, int takeCount, Guid iterationId)
        {
            throw new System.NotImplementedException();
        }
    }
}