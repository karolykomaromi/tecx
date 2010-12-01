using Microsoft.Practices.Prism.Commands;

namespace TecX.Agile.Infrastructure
{
    public static class Commands
    {
        public static readonly CompositeCommand HighlightField = new CompositeCommand();
        public static readonly CompositeCommand AddStoryCard = new CompositeCommand();
        public static readonly CompositeCommand RemoveStoryCard = new CompositeCommand();
        public static readonly CompositeCommand UpdateProperty = new CompositeCommand();
    }

    public class CommandProxy
    {
        public CompositeCommand HighlightField
        {
            get { return Commands.HighlightField; }
        }

        public CompositeCommand AddStoryCard
        {
            get { return Commands.AddStoryCard; }
        }

        public CompositeCommand RemoveStoryCard
        {
            get { return Commands.RemoveStoryCard; }
        }

        public CompositeCommand UpdateStoryCard
        {
            get { return Commands.UpdateProperty; }
        }
    }
}
