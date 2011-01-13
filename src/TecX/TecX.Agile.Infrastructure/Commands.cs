using Microsoft.Practices.Prism.Commands;

namespace TecX.Agile.Infrastructure
{
    public static class Commands
    {
        public static readonly CompositeCommand HighlightField = new CompositeCommand();
        public static readonly CompositeCommand MoveCaret = new CompositeCommand();

        public static readonly CompositeCommand AddStoryCard = new CompositeCommand();
        public static readonly CompositeCommand RemoveStoryCard = new CompositeCommand();
        public static readonly CompositeCommand UpdateProperty = new CompositeCommand();
        public static readonly CompositeCommand MoveStoryCard = new CompositeCommand();

        public static readonly CompositeCommand DisplayText = new CompositeCommand();

        public static readonly CompositeCommand Undo = new CompositeCommand();
        public static readonly CompositeCommand Redo = new CompositeCommand();
    }
}
