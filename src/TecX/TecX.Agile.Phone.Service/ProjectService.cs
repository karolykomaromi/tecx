namespace TecX.Agile.Phone.Service
{
    public class ProjectService : IProjectService
    {
        public ProjectQueryResult GetProjects(int maxResultCount)
        {
            return new ProjectQueryResult();
        }

        public IterationQueryResult GetIterations(int maxResultCount, int projectId)
        {
            return new IterationQueryResult();
        }

        public StoryQueryResult GetUserStories(int maxResultCount, int iterationId)
        {
            return new StoryQueryResult();
        }
    }
}