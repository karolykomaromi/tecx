namespace TecX.Agile.Service
{
    using System;

    using TecX.Agile.Service.Results;

    public class ProjectService : IProjectService
    {
        public QueryProjectsResult GetProjects(int startingFromIndex, int takeCount)
        {
            return new QueryProjectsResult
                {
                    Projects = new Project[0], 
                    TotalResultCount = 0
                };
        }

        public QueryIterationsResult GetIterations(int startingFromIndex, int takeCount, Guid projectId)
        {
            return new QueryIterationsResult
                {
                    Iterations = new Iteration[0], 
                    TotalResultCount = 0
                };
        }

        public QueryStoriesResult GetUserStories(int startingFromIndex, int takeCount, Guid iterationId)
        {
            return new QueryStoriesResult
                {
                    Stories = new StoryCard[0], 
                    TotalResultCount = 0
                };
        }

        public AddProjectResult AddProject(Project newProject)
        {
            return new AddProjectResult();
        }

        public AddIterationResult AddIteration(Iteration newIteration)
        {
            return new AddIterationResult();
        }

        public AddStoryResult AddStory(StoryCard newStory)
        {
            return new AddStoryResult();
        }

        public RemoveProjectResult RemoveProject(Guid projectId)
        {
            return new RemoveProjectResult();
        }

        public RemoveIterationResult RemoveIteration(Guid iterationId)
        {
            return new RemoveIterationResult();
        }

        public RemoveStoryResult RemoveStory(Guid storyId)
        {
            return new RemoveStoryResult();
        }
    }
}