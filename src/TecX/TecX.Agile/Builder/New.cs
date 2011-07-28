namespace TecX.Agile.Builder
{
    public static class New
    {
        public static StoryCardBuilder StoryCard()
        {
            return new StoryCardBuilder();
        }

        public static IterationBuilder Iteration()
        {
            return new IterationBuilder();
        }

        public static BacklogBuilder Backlog()
        {
            return new BacklogBuilder();
        }

        public static ProjectBuilder Project()
        {
            return new ProjectBuilder();
        }

        public static VisualizableBuilder View()
        {
            return new VisualizableBuilder();
        }

        public static TrackableBuilder Tracking()
        {
            return new TrackableBuilder();
        }

        public static LegendBuilder Legend()
        {
            return new LegendBuilder();
        }
    }
}