using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Converts model objects to builder
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts a <see cref="StoryCardBuilder"/> to a <see cref="StoryCard"/>
        /// </summary>
        public static StoryCardBuilder BuildUp(this StoryCard storycard)
        {
            Guard.AssertNotNull(storycard, "storycard");

            var builder = new StoryCardBuilder(storycard);

            return builder;
        }

        /// <summary>
        /// Converts a <see cref="Iteration"/> to an <see cref="IterationBuilder"/>
        /// </summary>
        public static IterationBuilder BuildUp(this Iteration iteration)
        {
            Guard.AssertNotNull(iteration, "iteration");

            var builder = new IterationBuilder(iteration);

            return builder;
        }

        /// <summary>
        /// Converts a <see cref="Backlog"/> to a <see cref="BacklogBuilder"/>
        /// </summary>
        public static BacklogBuilder BuildUp(this Backlog backlog)
        {
            Guard.AssertNotNull(backlog, "backlog");

            var builder = new BacklogBuilder(backlog);

            return builder;
        }

        /// <summary>
        /// Converts a <see cref="Project"/> to a <see cref="ProjectBuilder"/>
        /// </summary>
        public static ProjectBuilder BuildUp(this Project project)
        {
            Guard.AssertNotNull(project, "project");

            var builder = new ProjectBuilder(project);

            return builder;
        }

        public static LegendBuilder BuildUp(this Legend legend)
        {
            Guard.AssertNotNull(legend, "legend");

            return new LegendBuilder(legend);
        }

        public static VisualizableBuilder BuildUp(this Visualizable visualizable)
        {
            Guard.AssertNotNull(visualizable, "visualizable");

            return new VisualizableBuilder(visualizable);
        }

        public static TrackableBuilder BuildUp(this Trackable trackable)
        {
            Guard.AssertNotNull(trackable, "trackable");

            return new TrackableBuilder(trackable);
        }
    }
}